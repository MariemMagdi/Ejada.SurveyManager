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
        public QuestionAppService(
            IRepository<Question,Guid> repository,
            IRepository<Option, Guid> optionRepository)
            : base(repository)
        {
            _optionRepository = optionRepository;
            //GetPolicyName = "Questions.Default";
            //GetListPolicyName = "Questions.Default";
            //CreatePolicyName = "Questions.Create";
            //UpdatePolicyName = "Questions.Update";
            //DeletePolicyName = "Questions.Delete";

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

            var dto = ObjectMapper.Map<Question, QuestionDto>(question);
            dto.Options = ObjectMapper.Map<List<Option>, List<OptionDto>>(options);

            return dto;
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
                if (input.Options != null || input.Options.Any())
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

            var optionEntites = input.Options.Select(o => Option.Create(
                GuidGenerator.Create(),
                questionDto.Id,
                o.Label,
                o.Type)).ToList();
            if (optionEntites.Any())
            {
                await _optionRepository.InsertManyAsync(optionEntites, autoSave: true);
            }

            return await GetAsync(questionDto.Id);
        }

        public override async Task<QuestionDto> UpdateAsync(Guid id, UpdateQuestionDto input)
        {
            var questionDto = await base.UpdateAsync(id, input);
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
            var existingList = await _optionRepository.GetListAsync(o => o.QuestionId == id);
            var existingDict = existingList.ToDictionary(o => o.Id, o => o);

            var incomingIds = new HashSet<Guid>();

            foreach(var oDto in input.Options)
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

            return await GetAsync(id);
        }
    }
}
