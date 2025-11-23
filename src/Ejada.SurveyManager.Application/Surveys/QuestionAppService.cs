using Ejada.SurveyManager.Indicators;
using Ejada.SurveyManager.Permissions;
using Ejada.SurveyManager.Surveys.Dtos;
using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ejada.SurveyManager.Surveys
{
    public class QuestionAppService : CrudAppService<
        Question,
        QuestionDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateQuestionDto,
        UpdateQuestionDto>,
        IQuestionAppService
    {
        private readonly IRepository<Option, Guid> _optionRepository;
        private readonly IRepository<SurveyQuestion, Guid> _surveyQuestionRepository;
        private readonly IRepository<QuestionIndicator> _questionIndicatorRepository;
        private readonly IRepository<Survey, Guid> _surveyRepository;
        
        public QuestionAppService(
            IRepository<Question,Guid> repository,
            IRepository<Option, Guid> optionRepository,
            IRepository<SurveyQuestion, Guid> surveyQuestionRepository,
            IRepository<QuestionIndicator> questionIndicatorRepository,
            IRepository<Survey, Guid> surveyRepository)
            : base(repository)
        {
            _optionRepository = optionRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _questionIndicatorRepository = questionIndicatorRepository;
            _surveyRepository = surveyRepository;
            
            GetPolicyName = SurveyManagerPermissions.Questions.Default;
            GetListPolicyName = SurveyManagerPermissions.Questions.Default;
            CreatePolicyName = SurveyManagerPermissions.Questions.Create;
            UpdatePolicyName = SurveyManagerPermissions.Questions.Edit;
            DeletePolicyName = SurveyManagerPermissions.Questions.Delete;
        }

        protected override Task<Question> MapToEntityAsync(CreateQuestionDto createInput)
        {
            var question = Question.Create(
                GuidGenerator.Create(),
                createInput.Text,
                createInput.Type
            );
            return Task.FromResult(question);
        }

        private static bool IsChoiceType(QuestionType type)
        {
            return type == QuestionType.SingleChoice ||
                type == QuestionType.MultiChoice;
        }
        private static bool IsLikertType(QuestionType type)
        {
            return type == QuestionType.Likert1To5 ||
                type == QuestionType.Likert1To7;
        }
        public override async Task<QuestionDto> GetAsync(Guid id)
        {
            var question = await Repository.GetAsync(id);

            var options = await _optionRepository.GetListAsync(o => o.QuestionId == id);
            var questionIndicators = await _questionIndicatorRepository.GetListAsync(qi => qi.QuestionId == id);

            var dto = ObjectMapper.Map<Question, QuestionDto>(question);
            dto.Options = ObjectMapper.Map<List<Option>, List<OptionDto>>(options);
            
            // Check if linked to any non-deleted surveys
            var surveyQuestionLinks = await _surveyQuestionRepository.GetListAsync(sq => sq.QuestionId == id);
            if (surveyQuestionLinks.Any())
            {
                var surveyIds = surveyQuestionLinks.Select(sq => sq.SurveyId).ToList();
                dto.IsLinkedToSurvey = await _surveyRepository.AnyAsync(s => surveyIds.Contains(s.Id) && !s.IsDeleted);
            }
            else
            {
                dto.IsLinkedToSurvey = false;
            }
            
            dto.IndicatorIds = questionIndicators.Select(qi => qi.IndicatorId).ToList();

            return dto;
        }

        public override async Task<PagedResultDto<QuestionDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var result = await base.GetListAsync(input);
            
            // Get all question IDs from the result
            var questionIds = result.Items.Select(q => q.Id).ToList();
            
            // Get all survey-question links for these questions
            var surveyQuestionLinks = await _surveyQuestionRepository
                .GetListAsync(sq => questionIds.Contains(sq.QuestionId));
            
            // Get the survey IDs from the links
            var surveyIds = surveyQuestionLinks.Select(sq => sq.SurveyId).Distinct().ToList();
            
            // Get only non-deleted surveys
            var activeSurveys = await _surveyRepository
                .GetListAsync(s => surveyIds.Contains(s.Id) && !s.IsDeleted);
            
            var activeSurveyIds = activeSurveys.Select(s => s.Id).ToHashSet();
            
            // Filter links to only include those pointing to active (non-deleted) surveys
            var linkedQuestionIdSet = surveyQuestionLinks
                .Where(sq => activeSurveyIds.Contains(sq.SurveyId))
                .Select(sq => sq.QuestionId)
                .ToHashSet();
            
            // Get all question-indicator links for these questions
            var questionIndicators = await _questionIndicatorRepository
                .GetListAsync(qi => questionIds.Contains(qi.QuestionId));
            
            var indicatorsByQuestion = questionIndicators
                .GroupBy(qi => qi.QuestionId)
                .ToDictionary(g => g.Key, g => g.Select(qi => qi.IndicatorId).ToList());
            
            // Mark questions as linked and load indicator IDs
            foreach (var question in result.Items)
            {
                question.IsLinkedToSurvey = linkedQuestionIdSet.Contains(question.Id);
                question.IndicatorIds = indicatorsByQuestion.TryGetValue(question.Id, out var indicatorIds) 
                    ? indicatorIds 
                    : new List<Guid>();
            }
            
            return result;
        }

        public override async Task<QuestionDto> CreateAsync(CreateQuestionDto input)
        {
            if (IsChoiceType(input.Type))
            {
                if(input.Options == null || !input.Options.Any())
                {
                    throw new BusinessException("Question.Options.RequiredForChoiceType")
                        .WithData("QuestionType", input.Type.ToString());
                }
            }
            if (IsLikertType(input.Type))
            {
                if (input.Options != null && input.Options.Any())
                {
                    throw new BusinessException("Question.Options.NotAllowedForLikertType")
                        .WithData("QuestionType", input.Type.ToString());
                }
            }
            var questionDto = await base.CreateAsync(input);

            // For Likert types → no options in DB
            if (IsLikertType(input.Type))
            {
                return await GetAsync(questionDto.Id); // will return with empty Options
            }

            // For choice types → we already validated Options is non-null & non-empty
            if (input.Options == null || !input.Options.Any())
            {
                // Safety guard, though we already checked above
                return await GetAsync(questionDto.Id);
            }

            // Filter out options with empty labels before creating
            var validOptions = input.Options.Where(o => !string.IsNullOrWhiteSpace(o.Label)).ToList();
            
            if (!validOptions.Any())
            {
                throw new BusinessException("Question.Options.RequiredForChoiceType")
                    .WithData("QuestionType", input.Type.ToString());
            }

            var optionEntites = validOptions.Select(o => Option.Create(
                GuidGenerator.Create(),
                questionDto.Id,
                o.Label,
                o.Type)).ToList();
            if (optionEntites.Any())
            {
                await _optionRepository.InsertManyAsync(optionEntites, autoSave: true);
            }

            // Link indicators to the question
            if (input.IndicatorIds != null && input.IndicatorIds.Any())
            {
                foreach (var indicatorId in input.IndicatorIds)
                {
                    var questionIndicator = new QuestionIndicator(questionDto.Id, indicatorId);
                    await _questionIndicatorRepository.InsertAsync(questionIndicator, autoSave: false);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return await GetAsync(questionDto.Id);
        }

        public override async Task<QuestionDto> UpdateAsync(Guid id, UpdateQuestionDto input)
        {
            // Check if the question is linked to any survey
            var isLinkedToSurvey = await _surveyQuestionRepository.AnyAsync(sq => sq.QuestionId == id);
            if (isLinkedToSurvey)
            {
                throw new BusinessException("Question.CannotEdit.LinkedToSurvey")
                    .WithData("QuestionId", id);
            }
            
            // Get the existing question
            var question = await Repository.GetAsync(id);
            
            // Update text and type using domain methods
            question.SetText(input.Text);
            question.SetType(input.Type);
            
            // Save the question
            await Repository.UpdateAsync(question, autoSave: true);
            
            if (IsLikertType(input.Type))
            {
                var existingOptions = await _optionRepository.GetListAsync(o => o.QuestionId == id);
                if (existingOptions.Any())
                {
                    await _optionRepository.DeleteManyAsync(existingOptions, autoSave: true);
                }
                return await GetAsync(id);
            }

            // From here on, we are in SingleChoice / MultiChoice zone.
            if (input.Options == null)
            {
                var existingOptions = await _optionRepository.GetListAsync(o => o.QuestionId == id);
                if (!existingOptions.Any())
                {
                    throw new BusinessException("Question.Options.RequiredForChoiceType")
                        .WithData("QuestionId", id)
                        .WithData("QuestionType", input.Type.ToString());
                }
                return await GetAsync(id);
            }

            //options are not null
            // Filter out options with empty labels
            var validOptions = input.Options.Where(o => !string.IsNullOrWhiteSpace(o.Label)).ToList();
            
            if (!validOptions.Any())
            {
                throw new BusinessException("Question.Options.RequiredForChoiceType")
                        .WithData("QuestionId", id)
                        .WithData("QuestionType", input.Type.ToString());
            }

            var existingList = await _optionRepository.GetListAsync(o => o.QuestionId == id);
            var existingDict = existingList.ToDictionary(o => o.Id, o => o);

            var incomingIds = new HashSet<Guid>();

            foreach(var oDto in validOptions)
            {
                if(oDto.Id != Guid.Empty && existingDict.TryGetValue(oDto.Id, out var existingOption))
                {
                    existingOption.UpdateTypeAndLabel(oDto.Type, oDto.Label);
                    incomingIds.Add(oDto.Id);
                }
                else
                {
                    var option = Option.Create(GuidGenerator.Create(), id, oDto.Label, oDto.Type);

                    await _optionRepository.InsertAsync(option, autoSave: true);
                    incomingIds.Add(option.Id);
                }
            }

            if(incomingIds.Count == 0)
            {
                throw new BusinessException("Question.Options.RequiredForChoiceType")
                        .WithData("QuestionId", id)
                        .WithData("QuestionType", input.Type.ToString());
            }

            var toDelete = existingList.Where(o => !incomingIds.Contains(o.Id)).ToList();
            if (toDelete.Any())
            {
                await _optionRepository.DeleteManyAsync(toDelete,autoSave: true);
            }

            // Update indicator links
            // 1. Remove all existing links
            var existingIndicatorLinks = await _questionIndicatorRepository.GetListAsync(qi => qi.QuestionId == id);
            await _questionIndicatorRepository.DeleteManyAsync(existingIndicatorLinks, autoSave: false);

            // 2. Add new links
            if (input.IndicatorIds != null && input.IndicatorIds.Any())
            {
                foreach (var indicatorId in input.IndicatorIds)
                {
                    var questionIndicator = new QuestionIndicator(id, indicatorId);
                    await _questionIndicatorRepository.InsertAsync(questionIndicator, autoSave: false);
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return await GetAsync(id);
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Check if the question is linked to any non-deleted survey
            var surveyQuestionLinks = await _surveyQuestionRepository.GetListAsync(sq => sq.QuestionId == id);
            if (surveyQuestionLinks.Any())
            {
                var surveyIds = surveyQuestionLinks.Select(sq => sq.SurveyId).ToList();
                var isLinkedToActiveSurvey = await _surveyRepository.AnyAsync(s => surveyIds.Contains(s.Id) && !s.IsDeleted);
                
                if (isLinkedToActiveSurvey)
                {
                    throw new BusinessException("Question.CannotDelete.LinkedToSurvey")
                        .WithData("QuestionId", id);
                }
            }

            // Delete all options associated with this question first
            var options = await _optionRepository.GetListAsync(o => o.QuestionId == id);
            if (options.Any())
            {
                await _optionRepository.DeleteManyAsync(options, autoSave: true);
            }

            // Delete all indicator links associated with this question
            var questionIndicators = await _questionIndicatorRepository.GetListAsync(qi => qi.QuestionId == id);
            if (questionIndicators.Any())
            {
                await _questionIndicatorRepository.DeleteManyAsync(questionIndicators, autoSave: true);
            }

            await base.DeleteAsync(id);
        }

        public async Task<List<string>> GetLinkedSurveyNamesAsync(Guid questionId)
        {
            // Get all survey IDs linked to this question
            var surveyQuestions = await _surveyQuestionRepository.GetListAsync(sq => sq.QuestionId == questionId);
            
            if (!surveyQuestions.Any())
            {
                return new List<string>();
            }

            var surveyIds = surveyQuestions.Select(sq => sq.SurveyId).ToList();
            
            // Get only non-deleted surveys
            var surveys = await _surveyRepository.GetListAsync(s => surveyIds.Contains(s.Id) && !s.IsDeleted);
            
            return surveys.Select(s => s.Name).OrderBy(n => n).ToList();
        }
    }
}
