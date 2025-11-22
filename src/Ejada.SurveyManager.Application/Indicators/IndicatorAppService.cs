using Ejada.SurveyManager.Indicators.Dtos;
using Ejada.SurveyManager.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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

        public IndicatorAppService(
            IRepository<Indicator, Guid> repository,
            IRepository<QuestionIndicator> questionIndicatorRepository)
            : base(repository)
        {
            _questionIndicatorRepository = questionIndicatorRepository;

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
                await CurrentUnitOfWork.SaveChangesAsync();
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

            await CurrentUnitOfWork.SaveChangesAsync();

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
    }
}

