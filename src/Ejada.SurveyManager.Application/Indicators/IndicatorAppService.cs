using Ejada.SurveyManager.Indicators.Dtos;
using Ejada.SurveyManager.Permissions;
using Ejada.SurveyManager.Surveys;
using Ejada.SurveyManager.Surveys.Dtos;
using Ejada.SurveyManager.Surveys.Enums;
using Ejada.SurveyManager.SurveyInstances;
using Ejada.SurveyManager.SurveyInstances.Enums;
using Microsoft.AspNetCore.Authorization;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Timing;

namespace Ejada.SurveyManager.Indicators
{
    public class IndicatorAppService : CrudAppService<
        Indicator,
        IndicatorDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateIndicatorDto,
        UpdateIndicatorDto>,
        IIndicatorAppService
    {
        private readonly IRepository<QuestionIndicator> _questionIndicatorRepository;
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IRepository<Option, Guid> _optionRepository;
        private readonly IRepository<Response, Guid> _responseRepository;
        private readonly IRepository<ResponseOption, Guid> _responseOptionRepository;
        private readonly IRepository<SurveyInstance, Guid> _surveyInstanceRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IClock _clock;

        public IndicatorAppService(
            IRepository<Indicator, Guid> repository,
            IRepository<QuestionIndicator> questionIndicatorRepository,
            IRepository<Question, Guid> questionRepository,
            IRepository<Option, Guid> optionRepository,
            IRepository<Response, Guid> responseRepository,
            IRepository<ResponseOption, Guid> responseOptionRepository,
            IRepository<SurveyInstance, Guid> surveyInstanceRepository,
            IIdentityUserRepository identityUserRepository,
            IClock clock)
            : base(repository)
        {
            _questionIndicatorRepository = questionIndicatorRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _responseRepository = responseRepository;
            _responseOptionRepository = responseOptionRepository;
            _surveyInstanceRepository = surveyInstanceRepository;
            _identityUserRepository = identityUserRepository;
            _clock = clock;

            GetPolicyName = SurveyManagerPermissions.Indicators.ViewAll;
            GetListPolicyName = SurveyManagerPermissions.Indicators.ViewAll;
            CreatePolicyName = SurveyManagerPermissions.Indicators.Create;
            UpdatePolicyName = SurveyManagerPermissions.Indicators.Edit;
            DeletePolicyName = SurveyManagerPermissions.Indicators.Delete;
        }

        public override async Task<IndicatorDto> GetAsync(Guid id)
        {
            var indicator = await Repository.GetAsync(id);
            var dto = ObjectMapper.Map<Indicator, IndicatorDto>(indicator);

            // Load linked question IDs
            var questionIndicators = await _questionIndicatorRepository.GetListAsync(qi => qi.IndicatorId == id);
            dto.QuestionIds = questionIndicators.Select(qi => qi.QuestionId).ToList();

            return dto;
        }

        public override async Task<PagedResultDto<IndicatorDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var result = await base.GetListAsync(input);

            // Load question IDs for each indicator
            var indicatorIds = result.Items.Select(i => i.Id).ToList();
            var allQuestionIndicators = await _questionIndicatorRepository.GetListAsync(qi => indicatorIds.Contains(qi.IndicatorId));
            
            var questionIndicatorsByIndicator = allQuestionIndicators
                .GroupBy(qi => qi.IndicatorId)
                .ToDictionary(g => g.Key, g => g.Select(qi => qi.QuestionId).ToList());

            foreach (var dto in result.Items)
            {
                if (questionIndicatorsByIndicator.TryGetValue(dto.Id, out var questionIds))
                {
                    dto.QuestionIds = questionIds;
                }
            }

            return result;
        }

        [Authorize(SurveyManagerPermissions.Indicators.Create)]
        public override async Task<IndicatorDto> CreateAsync(CreateIndicatorDto input)
        {
            var indicator = Indicator.Create(
                GuidGenerator.Create(),
                input.Name,
                input.Description,
                input.IsActive
            );

            await Repository.InsertAsync(indicator, autoSave: true);

            // Link questions to indicator
            if (input.QuestionIds != null && input.QuestionIds.Any())
            {
                foreach (var questionId in input.QuestionIds)
                {
                    var questionIndicator = new QuestionIndicator(questionId, indicator.Id);
                    await _questionIndicatorRepository.InsertAsync(questionIndicator, autoSave: false);
                }
                await CurrentUnitOfWork!.SaveChangesAsync();
            }

            var dto = ObjectMapper.Map<Indicator, IndicatorDto>(indicator);
            dto.QuestionIds = input.QuestionIds ?? new List<Guid>();
            return dto;
        }

        [Authorize(SurveyManagerPermissions.Indicators.Edit)]
        public override async Task<IndicatorDto> UpdateAsync(Guid id, UpdateIndicatorDto input)
        {
            var indicator = await Repository.GetAsync(id);

            indicator.SetName(input.Name)
                     .SetDescription(input.Description);

            if (input.IsActive)
                indicator.Activate();
            else
                indicator.Deactivate();

            await Repository.UpdateAsync(indicator, autoSave: true);

            // Update question links
            // 1. Remove all existing links
            var existingLinks = await _questionIndicatorRepository.GetListAsync(qi => qi.IndicatorId == id);
            await _questionIndicatorRepository.DeleteManyAsync(existingLinks, autoSave: false);

            // 2. Add new links
            if (input.QuestionIds != null && input.QuestionIds.Any())
            {
                foreach (var questionId in input.QuestionIds)
                {
                    var questionIndicator = new QuestionIndicator(questionId, id);
                    await _questionIndicatorRepository.InsertAsync(questionIndicator, autoSave: false);
                }
            }

            await CurrentUnitOfWork!.SaveChangesAsync();

            var dto = ObjectMapper.Map<Indicator, IndicatorDto>(indicator);
            dto.QuestionIds = input.QuestionIds ?? new List<Guid>();
            return dto;
        }

        [Authorize(SurveyManagerPermissions.Indicators.Delete)]
        public override async Task DeleteAsync(Guid id)
        {
            // Delete all question links first
            var questionIndicators = await _questionIndicatorRepository.GetListAsync(qi => qi.IndicatorId == id);
            await _questionIndicatorRepository.DeleteManyAsync(questionIndicators, autoSave: true);

            // Delete the indicator
            await base.DeleteAsync(id);
        }

        [Authorize(SurveyManagerPermissions.Indicators.ViewAll)]
        public async Task<Stream> ExportIndicatorReportPdfAsync(Guid indicatorId, bool exportAllResponses = true)
        {
            // Load indicator
            var indicator = await Repository.GetAsync(indicatorId);
            
            // Load linked questions
            var questionIndicators = await _questionIndicatorRepository.GetListAsync(qi => qi.IndicatorId == indicatorId);
            var questionIds = questionIndicators.Select(qi => qi.QuestionId).ToList();
            
            if (!questionIds.Any())
            {
                throw new BusinessException("Indicator has no linked questions");
            }

            var questions = await _questionRepository.GetListAsync(q => questionIds.Contains(q.Id));
            var questionsById = questions.ToDictionary(q => q.Id);

            // Load options for all questions
            var options = await _optionRepository.GetListAsync(o => questionIds.Contains(o.QuestionId));
            var optionsByQuestion = options.GroupBy(o => o.QuestionId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Load all responses for these questions
            var responses = await _responseRepository.GetListAsync(r => questionIds.Contains(r.QuestionId));
            
            // Load response options
            var responseIds = responses.Select(r => r.Id).ToList();
            var responseOptions = responseIds.Any() 
                ? await _responseOptionRepository.GetListAsync(ro => responseIds.Contains(ro.ResponseId))
                : new List<ResponseOption>();
            var responseOptionsByResponse = responseOptions.GroupBy(ro => ro.ResponseId)
                .ToDictionary(g => g.Key, g => g.Select(ro => ro.OptionId).ToList());

            // Load survey instances
            var surveyInstanceIds = responses.Select(r => r.SurveyInstanceId).Distinct().ToList();
            var surveyInstances = surveyInstanceIds.Any()
                ? await _surveyInstanceRepository.GetListAsync(si => surveyInstanceIds.Contains(si.Id))
                : new List<SurveyInstance>();
            var instancesById = surveyInstances.ToDictionary(si => si.Id);

            // Load users
            var userIds = surveyInstances.Select(si => si.AssigneeUserId).Distinct().ToList();
            var users = new Dictionary<Guid, Volo.Abp.Identity.IdentityUser>();
            foreach (var userId in userIds)
            {
                try
                {
                    var user = await _identityUserRepository.GetAsync(userId);
                    users[userId] = user;
                }
                catch
                {
                    // User not found, skip
                }
            }

            // Calculate statistics
            var questionStats = new List<QuestionStat>();
            foreach (var question in questions)
            {
                var questionResponses = responses.Where(r => r.QuestionId == question.Id).ToList();
                var submittedResponses = questionResponses
                    .Where(r => instancesById.ContainsKey(r.SurveyInstanceId) && 
                                instancesById[r.SurveyInstanceId].Status == SurveyInstanceStatus.Submitted)
                    .ToList();

                double? questionAverage = null;
                if ((question.Type == QuestionType.Likert1To5 || question.Type == QuestionType.Likert1To7) && 
                    submittedResponses.Any(r => r.AnswerValue.HasValue))
                {
                    var likertResponses = submittedResponses.Where(r => r.AnswerValue.HasValue).ToList();
                    questionAverage = likertResponses.Average(r => r.AnswerValue!.Value);
                }

                questionStats.Add(new QuestionStat
                {
                    Question = question,
                    TotalResponses = questionResponses.Count,
                    SubmittedResponses = submittedResponses.Count,
                    Average = questionAverage
                });
            }

            var totalResponses = responses.Count;

            // Generate PDF
            var stream = new MemoryStream();
            
            QuestPDF.Settings.License = LicenseType.Community;
            
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content()
                        .Column(column =>
                        {
                            // Cover Page
                            column.Item().PaddingBottom(20).Column(cover =>
                            {
                                cover.Item().Text("Indicator Performance Report").FontSize(24).SemiBold().AlignCenter().FontColor(Colors.Blue.Medium);
                                cover.Item().PaddingTop(10).Text(indicator.Name).FontSize(18).SemiBold().AlignCenter();
                                cover.Item().PaddingTop(20).Text($"Generated: {_clock.Now:yyyy-MM-dd HH:mm:ss}").FontSize(10).AlignCenter();
                                cover.Item().Text("Prepared by: Ejada Survey Manager System").FontSize(10).AlignCenter();
                            });

                            // Indicator Summary
                            column.Item().PaddingTop(30).Column(summary =>
                            {
                                summary.Item().Text("Indicator Summary").FontSize(16).Bold().FontColor(Colors.Blue.Medium);
                                summary.Item().PaddingTop(5).Text($"Name: {indicator.Name}").FontSize(11);
                                if (!string.IsNullOrEmpty(indicator.Description))
                                {
                                    summary.Item().PaddingTop(2).Text($"Description: {indicator.Description}").FontSize(11);
                                }
                                summary.Item().PaddingTop(2).Text($"Total Questions: {questions.Count}").FontSize(11);
                                summary.Item().PaddingTop(2).Text($"Total Responses: {totalResponses}").FontSize(11);
                            });

                            // Questions Overview Table
                            column.Item().PaddingTop(20).Column(overview =>
                            {
                                overview.Item().Text("Questions Overview").FontSize(16).Bold().FontColor(Colors.Blue.Medium);
                                overview.Item().PaddingTop(5).Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(0.5f);
                                        columns.RelativeColumn(2f);
                                        columns.RelativeColumn(1f);
                                        columns.RelativeColumn(1f);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("#").FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("Question Text").FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("Question Average").FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("Responses").FontSize(9).Bold();
                                    });

                                    int questionNum = 1;
                                    foreach (var stat in questionStats)
                                    {
                                        table.Cell().Element(CellStyle).Text(questionNum.ToString()).FontSize(9);
                                        table.Cell().Element(CellStyle).Text(stat.Question.Text).FontSize(9);
                                        table.Cell().Element(CellStyle).Text(
                                            stat.Average.HasValue ? stat.Average.Value.ToString("F2") : "Not answered yet").FontSize(9);
                                        table.Cell().Element(CellStyle).Text(stat.SubmittedResponses.ToString()).FontSize(9);
                                        questionNum++;
                                    }
                                });
                            });

                            // Per-Question Details
                            int detailQuestionNum = 1;
                            foreach (var stat in questionStats)
                            {
                                column.Item().PageBreak();
                                column.Item().Column(detail =>
                                {
                                    detail.Item().Text($"Question {detailQuestionNum}: {stat.Question.Text}")
                                        .FontSize(14).Bold().FontColor(Colors.Blue.Medium);
                                    
                                    var questionResponses = responses.Where(r => r.QuestionId == stat.Question.Id).ToList();
                                    var responsesToExport = exportAllResponses 
                                        ? questionResponses 
                                        : questionResponses.Where(r => 
                                            instancesById.ContainsKey(r.SurveyInstanceId) && 
                                            instancesById[r.SurveyInstanceId].Status == SurveyInstanceStatus.Submitted).ToList();

                                    if (responsesToExport.Any())
                                    {
                                        detail.Item().PaddingTop(10).Table(table =>
                                        {
                                            table.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn(0.5f);
                                                columns.RelativeColumn(2f);
                                                columns.RelativeColumn(1.5f);
                                                columns.RelativeColumn(1f);
                                            });

                                            table.Header(header =>
                                            {
                                                header.Cell().Element(CellStyle).Text("#").FontSize(9).Bold();
                                                header.Cell().Element(CellStyle).Text("Employee Email").FontSize(9).Bold();
                                                header.Cell().Element(CellStyle).Text("Answer").FontSize(9).Bold();
                                                header.Cell().Element(CellStyle).Text("Response Date").FontSize(9).Bold();
                                            });

                                            int responseNum = 1;
                                            foreach (var response in responsesToExport)
                                            {
                                                var instance = instancesById.ContainsKey(response.SurveyInstanceId) 
                                                    ? instancesById[response.SurveyInstanceId] 
                                                    : null;
                                                var user = instance != null && users.ContainsKey(instance.AssigneeUserId)
                                                    ? users[instance.AssigneeUserId]
                                                    : null;
                                                var email = user?.Email ?? user?.UserName ?? "Unknown";

                                                string answerText = FormatAnswer(
                                                    stat.Question.Type,
                                                    response.AnswerValue,
                                                    responseOptionsByResponse.ContainsKey(response.Id) 
                                                        ? responseOptionsByResponse[response.Id].ToList() 
                                                        : new List<Guid>(),
                                                    optionsByQuestion.ContainsKey(stat.Question.Id)
                                                        ? optionsByQuestion[stat.Question.Id].ToDictionary(o => o.Id, o => o.Label)
                                                        : new Dictionary<Guid, string>());

                                                table.Cell().Element(CellStyle).Text(responseNum.ToString()).FontSize(8);
                                                table.Cell().Element(CellStyle).Text(email).FontSize(8);
                                                table.Cell().Element(CellStyle).Text(answerText).FontSize(8);
                                                table.Cell().Element(CellStyle).Text(response.CreationTime.ToString("yyyy-MM-dd")).FontSize(8);
                                                responseNum++;
                                            }
                                        });
                                    }
                                    else
                                    {
                                        detail.Item().PaddingTop(5).Text("No responses available yet.").FontSize(9).Italic();
                                    }
                                });
                                detailQuestionNum++;
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .DefaultTextStyle(style => style.FontSize(8).FontColor(Colors.Grey.Medium))
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" of ");
                            text.TotalPages();
                            text.Span($" | {indicator.Name}");
                        });
                });
            });

            document.GeneratePdf(stream);
            stream.Position = 0;
            return stream;
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(5)
                .PaddingHorizontal(5);
        }

        private string FormatAnswer(QuestionType questionType, int? answerValue, List<Guid> selectedOptionIds, Dictionary<Guid, string> optionMap)
        {
            switch (questionType)
            {
                case QuestionType.Likert1To5:
                case QuestionType.Likert1To7:
                    return answerValue.HasValue ? answerValue.Value.ToString() : "Not answered";

                case QuestionType.SingleChoice:
                    if (selectedOptionIds != null && selectedOptionIds.Any())
                    {
                        var optionId = selectedOptionIds.First();
                        return optionMap.TryGetValue(optionId, out var label) ? label : "Unknown option";
                    }
                    return "Not answered";

                case QuestionType.MultiChoice:
                    if (selectedOptionIds != null && selectedOptionIds.Any())
                    {
                        var labels = selectedOptionIds
                            .Select(id => optionMap.TryGetValue(id, out var label) ? label : "Unknown option")
                            .ToList();
                        return string.Join(", ", labels);
                    }
                    return "Not answered";

                default:
                    return "Unknown type";
            }
        }

        private class QuestionStat
        {
            public Question Question { get; set; } = null!;
            public int TotalResponses { get; set; }
            public int SubmittedResponses { get; set; }
            public double? Average { get; set; }
        }
    }
}

