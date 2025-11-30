using Ejada.SurveyManager.Indicators;
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
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Timing;

namespace Ejada.SurveyManager.Users
{
    [Authorize(SurveyManagerPermissions.Indicators.ViewAll)]
    public class UserReportAppService : ApplicationService, IUserReportAppService
    {
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IRepository<Indicator, Guid> _indicatorRepository;
        private readonly IRepository<QuestionIndicator> _questionIndicatorRepository;
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IRepository<Option, Guid> _optionRepository;
        private readonly IRepository<Response, Guid> _responseRepository;
        private readonly IRepository<ResponseOption, Guid> _responseOptionRepository;
        private readonly IRepository<SurveyInstance, Guid> _surveyInstanceRepository;
        private readonly IClock _clock;

        public UserReportAppService(
            IIdentityUserRepository identityUserRepository,
            IRepository<Indicator, Guid> indicatorRepository,
            IRepository<QuestionIndicator> questionIndicatorRepository,
            IRepository<Question, Guid> questionRepository,
            IRepository<Option, Guid> optionRepository,
            IRepository<Response, Guid> responseRepository,
            IRepository<ResponseOption, Guid> responseOptionRepository,
            IRepository<SurveyInstance, Guid> surveyInstanceRepository,
            IClock clock)
        {
            _identityUserRepository = identityUserRepository;
            _indicatorRepository = indicatorRepository;
            _questionIndicatorRepository = questionIndicatorRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _responseRepository = responseRepository;
            _responseOptionRepository = responseOptionRepository;
            _surveyInstanceRepository = surveyInstanceRepository;
            _clock = clock;
        }

        public async Task<Stream> ExportUserReportPdfAsync(Guid userId)
        {
            // Load user
            var user = await _identityUserRepository.GetAsync(userId);
            
            // Check if user is an Employee
            var userRoles = await _identityUserRepository.GetRolesAsync(userId);
            var isEmployee = userRoles.Any(r => r.Name == "Employee");
            
            if (!isEmployee)
            {
                throw new BusinessException("User is not an Employee. Reports can only be generated for Employee users.");
            }

            // Get all indicators
            var allIndicators = await _indicatorRepository.GetListAsync();
            
            // Get all question-indicator links
            var allQuestionIndicators = await _questionIndicatorRepository.GetListAsync();
            var questionIdsByIndicator = allQuestionIndicators
                .GroupBy(qi => qi.IndicatorId)
                .ToDictionary(g => g.Key, g => g.Select(qi => qi.QuestionId).ToList());

            // Get all questions
            var allQuestionIds = questionIdsByIndicator.Values.SelectMany(q => q).Distinct().ToList();
            var questions = allQuestionIds.Any()
                ? await _questionRepository.GetListAsync(q => allQuestionIds.Contains(q.Id))
                : new List<Question>();
            var questionsById = questions.ToDictionary(q => q.Id);

            // Get all options
            var options = allQuestionIds.Any()
                ? await _optionRepository.GetListAsync(o => allQuestionIds.Contains(o.QuestionId))
                : new List<Option>();
            var optionsByQuestion = options.GroupBy(o => o.QuestionId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Get all survey instances for this user
            var userSurveyInstances = await _surveyInstanceRepository.GetListAsync(si => si.AssigneeUserId == userId);
            var surveyInstanceIds = userSurveyInstances.Select(si => si.Id).ToList();

            // Get all responses for this user's survey instances
            var responses = surveyInstanceIds.Any()
                ? await _responseRepository.GetListAsync(r => surveyInstanceIds.Contains(r.SurveyInstanceId))
                : new List<Response>();
            var responsesByQuestion = responses.GroupBy(r => r.QuestionId)
                .ToDictionary(g => g.Key, g => g.FirstOrDefault()); // Take first response per question

            // Get response options
            var responseIds = responses.Select(r => r.Id).ToList();
            var responseOptions = responseIds.Any()
                ? await _responseOptionRepository.GetListAsync(ro => responseIds.Contains(ro.ResponseId))
                : new List<ResponseOption>();
            var responseOptionsByResponse = responseOptions.GroupBy(ro => ro.ResponseId)
                .ToDictionary(g => g.Key, g => g.Select(ro => ro.OptionId).ToList());

            // Filter indicators to only those where user has responses
            var indicatorsWithResponses = new List<Indicator>();
            foreach (var indicator in allIndicators)
            {
                if (questionIdsByIndicator.TryGetValue(indicator.Id, out var questionIds))
                {
                    var hasResponse = questionIds.Any(qId => responsesByQuestion.ContainsKey(qId));
                    if (hasResponse)
                    {
                        indicatorsWithResponses.Add(indicator);
                    }
                }
            }

            if (!indicatorsWithResponses.Any())
            {
                throw new BusinessException("User has no responses to any indicators.");
            }

            // Generate PDF
            var stream = new MemoryStream();
            
            QuestPDF.Settings.License = LicenseType.Community;
            
            var document = Document.Create(container =>
            {
                // Cover Page
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content()
                        .Column(column =>
                        {
                            column.Item().AlignCenter().Column(cover =>
                            {
                                cover.Item().PaddingTop(100).Text("User Performance Report").FontSize(24).Bold().AlignCenter();
                                cover.Item().PaddingTop(20).Text($"{user.Name} {user.Surname}").FontSize(18).SemiBold().AlignCenter();
                                cover.Item().PaddingTop(10).Text(user.Email ?? user.UserName).FontSize(14).AlignCenter();
                                cover.Item().PaddingTop(50).Text($"Generated: {_clock.Now:yyyy-MM-dd HH:mm:ss}").FontSize(10).AlignCenter();
                            });
                        });
                });

                // Per-Indicator Pages
                int indicatorNum = 1;
                foreach (var indicator in indicatorsWithResponses)
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        page.Header()
                            .Text("User Performance Report")
                            .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                        page.Content()
                            .Column(column =>
                            {
                                column.Item().Text($"Indicator {indicatorNum}: {indicator.Name}")
                                    .FontSize(16).Bold().FontColor(Colors.Blue.Medium);

                                if (questionIdsByIndicator.TryGetValue(indicator.Id, out var questionIds))
                                {
                                    var indicatorQuestions = questionIds
                                        .Where(qId => questionsById.ContainsKey(qId))
                                        .Select(qId => questionsById[qId])
                                        .Where(q => responsesByQuestion.ContainsKey(q.Id))
                                        .ToList();

                                    if (indicatorQuestions.Any())
                                    {
                                        column.Item().PaddingTop(10).Table(table =>
                                        {
                                            table.ColumnsDefinition(columns =>
                                            {
                                                columns.RelativeColumn(1f);
                                                columns.RelativeColumn(2f);
                                            });

                                            table.Header(header =>
                                            {
                                                header.Cell().Element(CellStyle).Text("Question").FontSize(9).Bold();
                                                header.Cell().Element(CellStyle).Text("Answer").FontSize(9).Bold();
                                            });

                                            foreach (var question in indicatorQuestions)
                                            {
                                                if (responsesByQuestion.TryGetValue(question.Id, out var response) && response != null)
                                                {
                                                    string answerText = FormatAnswer(
                                                        question.Type,
                                                        response.AnswerValue,
                                                        responseOptionsByResponse.ContainsKey(response.Id)
                                                            ? responseOptionsByResponse[response.Id].ToList()
                                                            : new List<Guid>(),
                                                        optionsByQuestion.ContainsKey(question.Id)
                                                            ? optionsByQuestion[question.Id].ToDictionary(o => o.Id, o => o.Label)
                                                            : new Dictionary<Guid, string>());

                                                    table.Cell().Element(CellStyle).Text(question.Text).FontSize(9);
                                                    table.Cell().Element(CellStyle).Text(answerText).FontSize(9);
                                                }
                                            }
                                        });
                                    }
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
                                text.Span($" | {user.Name} {user.Surname}");
                            });
                    });
                    indicatorNum++;
                }
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
    }
}

