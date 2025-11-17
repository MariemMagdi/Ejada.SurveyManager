using Ejada.SurveyManager.Surveys.Dtos;
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


        public SurveyAppService(
            IRepository<Survey, Guid> surveyRepository,
            IRepository<Question, Guid> questionRepository,
            IRepository<SurveyQuestion,Guid> surveyQuestionRepository)
            : base(surveyRepository)
        {
            _questionRepository = questionRepository;
            _surveyQuestionRepository = surveyQuestionRepository;

            GetPolicyName = "Surveys.Default";
            GetListPolicyName = "Surveys.Default";
            CreatePolicyName = "Surveys.Create";
            UpdatePolicyName = "Surveys.Update";
            DeletePolicyName = "Surveys.Delete";
        }

        protected override async Task<Survey> MapToEntityAsync(CreateSurveyDto createInput)
        {
            if (CurrentUser == null || CurrentUser.Id == null)
                throw new BusinessException("Survey.CreatorId.MissingUser");
            var survey = Survey.Create(
                GuidGenerator.Create(),
                createInput.Name,
                createInput.Purpose,
                createInput.TargetAudience,
                createInput.IsActive);

            return survey;
        }

        public override async Task<SurveyDto> CreateAsync(CreateSurveyDto input)
        {
            var surveyDto = await base.CreateAsync(input);

            if(input.ExistingQuestionIds!=null && input.ExistingQuestionIds.Any())
            {
                var links = input.ExistingQuestionIds
                    .Distinct()
                    .Select(qId => new SurveyQuestion(
                        GuidGenerator.Create(), surveyDto.Id, qId))
                    .ToList();

                if (links.Any())
                {
                    await _surveyQuestionRepository.InsertManyAsync(links, autoSave: true);
                }
            }

            return surveyDto;
        }

        public override async Task<SurveyDto> UpdateAsync(Guid id, UpdateSurveyDto input)
        {
            var surveyDto = await base.UpdateAsync(id, input);

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

            //Handle attaching questions (add links if not already linked)
            if(input.AttachQuestionIds != null && input.AttachQuestionIds.Any())
            {
                var attachIds = input.AttachQuestionIds.Distinct().ToList();

                //Finding existing links
                var existingLinks = await _surveyQuestionRepository.GetListAsync(
                    sq => sq.SurveyId == id && attachIds.Contains(sq.QuestionId));
                var alreadyLinkedIds = existingLinks.Select(sq => sq.QuestionId).ToHashSet();

                var newLinks = attachIds
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
    }
}
