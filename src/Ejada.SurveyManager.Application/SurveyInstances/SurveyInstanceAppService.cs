using Ejada.SurveyManager.Permissions;
using Ejada.SurveyManager.SurveyInstances.Dtos;
using Ejada.SurveyManager.SurveyInstances.Enums;
using Ejada.SurveyManager.Surveys;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Ejada.SurveyManager.SurveyInstances
{
    public class SurveyInstanceAppService : CrudAppService<
        SurveyInstance,
        SurveyInstanceDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateSurveyInstanceDto,
        UpdateSurveyInstanceDto>,
        ISurveyInstanceAppService
    {
        private readonly IRepository<Survey, Guid> _surveyRepository;

        public SurveyInstanceAppService(
            IRepository<SurveyInstance,Guid> repository,
            IRepository<Survey, Guid> surveyRepository)
            : base(repository)
        {
            _surveyRepository = surveyRepository;
            
            GetPolicyName = SurveyManagerPermissions.SurveyInstances.ViewAll;
            GetListPolicyName = SurveyManagerPermissions.SurveyInstances.ViewAll;
            CreatePolicyName = SurveyManagerPermissions.SurveyInstances.Create;
            UpdatePolicyName = SurveyManagerPermissions.SurveyInstances.Edit;
            DeletePolicyName = SurveyManagerPermissions.SurveyInstances.Delete;
        }

        protected override Task<SurveyInstance> MapToEntityAsync(CreateSurveyInstanceDto createInput)
        {
            var entity = SurveyInstance.Create(
                GuidGenerator.Create(),
                createInput.SurveyId,
                createInput.AssigneeUserId,
                createInput.DueDate);
            
            return Task.FromResult(entity);
        }

        public override async Task<PagedResultDto<SurveyInstanceDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var result = await base.GetListAsync(input);
            
            // Load survey names
            var surveyIds = result.Items.Select(x => x.SurveyId).Distinct().ToList();
            var surveys = await _surveyRepository.GetListAsync(s => surveyIds.Contains(s.Id));
            var surveyDict = surveys.ToDictionary(s => s.Id, s => s.Name);
            
            // Populate survey names
            foreach (var item in result.Items)
            {
                if (surveyDict.TryGetValue(item.SurveyId, out var surveyName))
                {
                    item.SurveyName = surveyName;
                }
            }
            
            return result;
        }

        public override async Task<SurveyInstanceDto> GetAsync(Guid id)
        {
            var result = await base.GetAsync(id);
            
            // Load survey name
            var survey = await _surveyRepository.GetAsync(result.SurveyId);
            result.SurveyName = survey.Name;
            
            return result;
        }
        public async Task<SurveyInstanceDto> MarkInProgressAsync(Guid id) {
            var entity = await Repository.GetAsync(id);
            if (entity.Status == SurveyInstanceStatus.Submitted ||
                entity.Status == SurveyInstanceStatus.Expired)
            {
                throw new BusinessException("SurveyInstance.InProgress.NotAllowedFromFinalState")
                    .WithData("Id", id)
                    .WithData("Status", entity.Status.ToString());
            }

            //Check DueDate First
            if(entity.DueDate.HasValue && DateTime.UtcNow >= entity.DueDate.Value)
            {
                entity.MarkExpired();
                await Repository.UpdateAsync(entity, autoSave: true);
                throw new BusinessException("SurveyInstance.InProgress.DueDatePassed")
                    .WithData("Id", id)
                    .WithData("DueDate", entity.DueDate.Value);
            }

            entity.MarkInProgress();
            await Repository.UpdateAsync(entity, autoSave: true);

            return MapToGetOutputDto(entity);
        }
        public async Task<SurveyInstanceDto> SubmitAsync(Guid id) 
        {
            var entity = await Repository.GetAsync(id);
            if (entity.Status == SurveyInstanceStatus.Submitted)
            {
                return MapToGetOutputDto(entity);
            }

            if (entity.Status == SurveyInstanceStatus.Expired)
            {
                throw new BusinessException("SurveyInstance.Submit.Expired")
                    .WithData("Id", id)
                    .WithData("DueDate", entity.DueDate.Value);
            }

            if (entity.DueDate.HasValue && DateTime.UtcNow >= entity.DueDate.Value)
            {
                entity.MarkExpired();
                await Repository.UpdateAsync(entity, autoSave: true);
                throw new BusinessException("SurveyInstance.InProgress.DueDatePassed")
                    .WithData("Id", id)
                    .WithData("DueDate", entity.DueDate.Value);
            }

            entity.MarkSubmitted();
            await Repository.UpdateAsync(entity, autoSave: true);

            return MapToGetOutputDto(entity);
        }
        [Authorize(SurveyManagerPermissions.SurveyInstances.MarkExpired)]
        public async Task<SurveyInstanceDto> MarkExpiredAsync(Guid id) 
        {
            var entity = await Repository.GetAsync(id);
            if (entity.Status == SurveyInstanceStatus.Submitted)
            {
                throw new BusinessException("SurveyInstance.Expire.SubmittedNotAllowed")
                    .WithData("Id", id);
            }

            entity.MarkExpired();
            await Repository.UpdateAsync(entity, autoSave: true);
            return MapToGetOutputDto(entity);   
        }

        [Authorize(SurveyManagerPermissions.SurveyInstances.ViewOwn)]
        public async Task<PagedResultDto<SurveyInstanceDto>> GetMyAssignedSurveysAsync(PagedAndSortedResultRequestDto input)
        {
            var currentUserId = CurrentUser.GetId();
            
            // Get all survey instances assigned to current user
            var query = await Repository.GetQueryableAsync();
            query = query.Where(si => si.AssigneeUserId == currentUserId);
            
            // Apply sorting
            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Simple sorting - you can enhance this if needed
                query = query.OrderBy(si => si.CreationTime);
            }
            else
            {
                query = query.OrderByDescending(si => si.CreationTime);
            }
            
            // Get total count
            var totalCount = query.Count();
            
            // Apply paging
            var items = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();
            
            // Map to DTOs and populate survey names
            var dtos = ObjectMapper.Map<List<SurveyInstance>, List<SurveyInstanceDto>>(items);
            
            // Load survey names
            var surveyIds = dtos.Select(x => x.SurveyId).Distinct().ToList();
            var surveys = await _surveyRepository.GetListAsync(s => surveyIds.Contains(s.Id));
            var surveyDict = surveys.ToDictionary(s => s.Id, s => s.Name);
            
            foreach (var dto in dtos)
            {
                if (surveyDict.TryGetValue(dto.SurveyId, out var surveyName))
                {
                    dto.SurveyName = surveyName;
                }
            }
            
            return new PagedResultDto<SurveyInstanceDto>(totalCount, dtos);
        }
    }
}
