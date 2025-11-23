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
    public class SurveyAppService 
        : CrudAppService<
            Survey,
            SurveyDto,
            Guid,
            PagedAndSortedResultRequestDto,
            CreateSurveyDto,
            UpdateSurveyDto
            >,
        ISurveyAppService
    {
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IRepository<SurveyQuestion, Guid> _surveyQuestionRepository;
        private readonly IRepository<Option, Guid> _optionRepository;


        public SurveyAppService(
            IRepository<Survey, Guid> surveyRepository,
            IRepository<Question, Guid> questionRepository,
            IRepository<SurveyQuestion,Guid> surveyQuestionRepository,
            IRepository<Option, Guid> optionRepository)
            : base(surveyRepository)
        {
            _questionRepository = questionRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _optionRepository = optionRepository;

            GetPolicyName = SurveyManagerPermissions.Surveys.Default;
            GetListPolicyName = SurveyManagerPermissions.Surveys.Default;
            CreatePolicyName = SurveyManagerPermissions.Surveys.Create;
            UpdatePolicyName = SurveyManagerPermissions.Surveys.Edit;
            DeletePolicyName = SurveyManagerPermissions.Surveys.Delete;
            //CreatePolicyName = "Surveys.Create";
            //UpdatePolicyName = "Surveys.Update";
            //DeletePolicyName = "Surveys.Delete";
        }

        protected override Task<Survey> MapToEntityAsync(CreateSurveyDto createInput)
        {
            //if (CurrentUser == null || CurrentUser.Id == null)
            //    throw new BusinessException("Survey.CreatorId.MissingUser");
            var survey = Survey.Create(
                GuidGenerator.Create(),
                createInput.Name,
                createInput.Purpose,
                createInput.TargetAudience,
                createInput.IsActive);

            return Task.FromResult(survey);
        }

        protected override void MapToEntity(UpdateSurveyDto updateInput, Survey entity)
        {
            entity.SetName(updateInput.Name);
            entity.SetPurpose(updateInput.Purpose ?? string.Empty);
            entity.SetTargetAudience(updateInput.TargetAudience ?? string.Empty);
            if (updateInput.IsActive)
                entity.Activate();
            else
                entity.Deactivate();
        }

        public override async Task<SurveyDto> CreateAsync(CreateSurveyDto input)
        {
            // First create the survey
            var surveyDto = await base.CreateAsync(input);
            var questionIdsToLink = new List<Guid>();

            // Handle existing questions
            if(input.ExistingQuestionIds!=null && input.ExistingQuestionIds.Any())
            {
                questionIdsToLink.AddRange(input.ExistingQuestionIds.Distinct());
            }

            // Handle new inline questions
            if(input.NewQuestions != null && input.NewQuestions.Any())
            {
                foreach(var newQ in input.NewQuestions)
                {
                    // Create and insert question
                    var questionId = GuidGenerator.Create();
                    var question = Question.Create(questionId, newQ.Text, newQ.Type);
                    
                    await _questionRepository.InsertAsync(question, autoSave: true);
                    
                    // Handle options for choice-type questions
                    if(newQ.Options != null && newQ.Options.Any() && 
                       (newQ.Type == QuestionType.SingleChoice || newQ.Type == QuestionType.MultiChoice))
                    {
                        // Filter out options with empty labels
                        var validOptions = newQ.Options.Where(o => !string.IsNullOrWhiteSpace(o.Label)).ToList();
                        
                        if (validOptions.Any())
                        {
                            var options = validOptions.Select(o => 
                                Option.Create(GuidGenerator.Create(), questionId, o.Label, o.Type)
                            ).ToList();
                            
                            await _optionRepository.InsertManyAsync(options, autoSave: true);
                        }
                    }
                    
                    questionIdsToLink.Add(questionId);
                }
            }

            // Link all questions to survey
            if(questionIdsToLink.Any())
            {
                var links = questionIdsToLink
                    .Distinct()
                    .Select(qId => new SurveyQuestion(
                        GuidGenerator.Create(), surveyDto.Id, qId))
                    .ToList();

                await _surveyQuestionRepository.InsertManyAsync(links, autoSave: true);
            }

            return surveyDto;
        }

        public override async Task<SurveyDto> UpdateAsync(Guid id, UpdateSurveyDto input)
        {
            var surveyDto = await base.UpdateAsync(id, input);
            var questionIdsToAttach = new List<Guid>();

            //Handle detaching questions (remove links)
            if(input.DetachQuestionIds != null && input.DetachQuestionIds.Any())
            {
                var detachIds = input.DetachQuestionIds.Distinct().ToList();

                var linksToRemove = await _surveyQuestionRepository.GetListAsync(
                    sq => sq.SurveyId == id && detachIds.Contains(sq.QuestionId));

                if (linksToRemove.Any())
                {
                    await _surveyQuestionRepository.DeleteManyAsync(linksToRemove, autoSave: true);
                }
            }

            //Handle attaching existing questions
            if(input.AttachQuestionIds != null && input.AttachQuestionIds.Any())
            {
                questionIdsToAttach.AddRange(input.AttachQuestionIds.Distinct());
            }

            // Handle new inline questions
            if(input.NewQuestions != null && input.NewQuestions.Any())
            {
                foreach(var newQ in input.NewQuestions)
                {
                    var questionId = GuidGenerator.Create();
                    var question = Question.Create(questionId, newQ.Text, newQ.Type);
                    
                    await _questionRepository.InsertAsync(question, autoSave: true);
                    
                    // Handle options for choice-type questions
                    if(newQ.Options != null && newQ.Options.Any() && 
                       (newQ.Type == QuestionType.SingleChoice || newQ.Type == QuestionType.MultiChoice))
                    {
                        // Filter out options with empty labels
                        var validOptions = newQ.Options.Where(o => !string.IsNullOrWhiteSpace(o.Label)).ToList();
                        
                        if (validOptions.Any())
                        {
                            var options = validOptions.Select(o => 
                                Option.Create(GuidGenerator.Create(), questionId, o.Label, o.Type)
                            ).ToList();
                            
                            await _optionRepository.InsertManyAsync(options, autoSave: true);
                        }
                    }
                    
                    questionIdsToAttach.Add(questionId);
                }
            }

            // Handle updating existing questions (creates new question entity with updated data)
            if(input.UpdateQuestions != null && input.UpdateQuestions.Any())
            {
                foreach(var updateQ in input.UpdateQuestions)
                {
                    // Create a NEW question with the updated data
                    var newQuestionId = GuidGenerator.Create();
                    var newQuestion = Question.Create(newQuestionId, updateQ.Text, updateQ.Type);
                    
                    await _questionRepository.InsertAsync(newQuestion, autoSave: true);
                    
                    // Handle options
                    if(updateQ.Options != null && updateQ.Options.Any() && 
                       (updateQ.Type == QuestionType.SingleChoice || updateQ.Type == QuestionType.MultiChoice))
                    {
                        // Filter out options with empty labels
                        var validOptions = updateQ.Options.Where(o => !string.IsNullOrWhiteSpace(o.Label)).ToList();
                        
                        if (validOptions.Any())
                        {
                            var options = validOptions.Select(o => 
                                Option.Create(GuidGenerator.Create(), newQuestionId, o.Label, o.Type)
                            ).ToList();
                            
                            await _optionRepository.InsertManyAsync(options, autoSave: true);
                        }
                    }
                    
                    // Detach old question
                    var oldLink = await _surveyQuestionRepository.FirstOrDefaultAsync(
                        sq => sq.SurveyId == id && sq.QuestionId == updateQ.Id);
                    if(oldLink != null)
                    {
                        await _surveyQuestionRepository.DeleteAsync(oldLink, autoSave: true);
                    }
                    
                    // Attach new question
                    questionIdsToAttach.Add(newQuestionId);
                }
            }

            // Attach all new/updated questions
            if(questionIdsToAttach.Any())
            {
                //Finding existing links
                var existingLinks = await _surveyQuestionRepository.GetListAsync(
                    sq => sq.SurveyId == id && questionIdsToAttach.Contains(sq.QuestionId));
                var alreadyLinkedIds = existingLinks.Select(sq => sq.QuestionId).ToHashSet();

                var newLinks = questionIdsToAttach
                    .Where(qId => !alreadyLinkedIds.Contains(qId))
                    .Select(qId => new SurveyQuestion(
                        GuidGenerator.Create(), id, qId))
                    .ToList();

                if (newLinks.Any())
                {
                    await _surveyQuestionRepository.InsertManyAsync(newLinks, autoSave: true);
                }
            }

            return surveyDto;
        }

        public virtual async Task<SurveyWithQuestionsDto> GetWithQuestionAsync(Guid id) 
        {
            //Load the survey
            var survey = await Repository.GetAsync(id);

            //Load all questions linked to this survey
            var surveyQuestionQueryable = await _surveyQuestionRepository.GetQueryableAsync();
            var questionsQueryable = await _questionRepository.GetQueryableAsync();

            var questionQuery =
                from sq in surveyQuestionQueryable
                join q in questionsQueryable
                on sq.QuestionId equals q.Id
                where sq.SurveyId == id
                select q;

            var questionEntities = await AsyncExecuter.ToListAsync(questionQuery);

            //Load all options for these questions
            var questionIds = questionEntities.Select(q => q.Id).ToList();
            var optionList = questionIds.Any() ?
                await _optionRepository.GetListAsync(o => questionIds.Contains(o.QuestionId))
                : new List<Option>();
            var optionsByQuestion = optionList
                .GroupBy(o => o.QuestionId)
                .ToDictionary(g=>g.Key, g=>g.ToList());

            // Map Survey
            var dto = ObjectMapper.Map<Survey, SurveyWithQuestionsDto>(survey);
            dto.Questions = new List<QuestionDto>();

            //Map questions and attach its options
            foreach(var question in questionEntities)
            {
                var questionDto = ObjectMapper.Map<Question, QuestionDto>(question);
                if (optionsByQuestion.TryGetValue(question.Id, out var qOptions)) 
                {
                    questionDto.Options = ObjectMapper.Map<List<Option>, List<OptionDto>>(qOptions);
                }
                else
                {
                    questionDto.Options = new List<OptionDto>();
                }
                dto.Questions.Add(questionDto);
            }

            return dto;
        }
    }
}
