using Ejada.SurveyManager.SurveyInstances.Dtos;
using Ejada.SurveyManager.SurveyInstances.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

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
        public SurveyInstanceAppService(IRepository<SurveyInstance,Guid> repository)
            : base(repository)
        {
            // GetPolicyName = "SurveyInstances.Default";
            // GetListPolicyName = "SurveyInstances.Default";
            // CreatePolicyName = "SurveyInstances.Create";
            // UpdatePolicyName = "SurveyInstances.Update";
            // DeletePolicyName = "SurveyInstances.Delete";
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

        //public override async Task<SurveyInstanceDto> UpdateAsync(Guid id, UpdateSurveyInstanceDto input)
        //{
        //    var entity = await Repository.GetAsync(id);

        //    if(entity.Status == SurveyInstanceStatus.Submitted || entity.Status == SurveyInstanceStatus.Expired)
        //    {
        //        throw new BusinessException("SurveyInstance.Update.NotAllowedAfterLocked")
        //            .WithData("Id", id)
        //            .WithData("Status", entity.Status.ToString());
        //    }

            
        //}
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
    }
}
