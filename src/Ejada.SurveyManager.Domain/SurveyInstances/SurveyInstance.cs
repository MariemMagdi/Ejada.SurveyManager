using Ejada.SurveyManager.SurveyInstances.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Timing;

namespace Ejada.SurveyManager.SurveyInstances
{
    public class SurveyInstance : FullAuditedAggregateRoot<Guid>
    {
        public Guid SurveyId { get; private set; }
        public Guid AssigneeUserId { get; private set; }
        public DateTime? DueDate { get; private set; }

        public SurveyInstanceStatus Status { get; private set; }

        private SurveyInstance() { }

        private SurveyInstance(Guid id, Guid surveyId, Guid assigneeUserId, DateTime? dueDate, SurveyInstanceStatus status)
            : base(id)
        {
            SetSurvey(surveyId);
            SetAssigneeUser(assigneeUserId);
            SetDueDate(dueDate);
            Status = status;
        }

        public static SurveyInstance Create(Guid id, Guid surveyId, Guid assigneeUserId, DateTime? dueDate = null) 
        {
            return new SurveyInstance(id, surveyId, assigneeUserId, dueDate, SurveyInstanceStatus.Assigned);
        }

        public SurveyInstance SetSurvey(Guid surveyId) 
        {
            if (surveyId == Guid.Empty)
            {
                throw new BusinessException("SurveyInstance.SurveyId.Invalid")
                    .WithData("SurveyId", surveyId);
            }

            SurveyId = surveyId;
            return this;
        }

        public SurveyInstance SetAssigneeUser(Guid assigneeUserId) 
        {
            if(assigneeUserId == Guid.Empty)
            {
                throw new BusinessException("SurveyInstance.AssigneeUserId.Invalid")
                    .WithData("AssigneeUserId", assigneeUserId);
            }
            AssigneeUserId = assigneeUserId;
            return this;
        }

        public SurveyInstance SetDueDate(DateTime? dueDate) 
        {
            if (dueDate.HasValue)
            {
                if (dueDate.Value <= DateTime.UtcNow)
                {
                    throw new BusinessException("SurveyInstance.DueDate.MustBeFuture")
                .WithData("DueDate", dueDate.Value)
                .WithData("CurrentTime", DateTime.UtcNow);
                }
            }

            DueDate = dueDate;
            return this;
        }

        public SurveyInstance MarkAssigned()
        {
            Status = SurveyInstanceStatus.Assigned;
            return this;
        }

        public SurveyInstance MarkInProgress()
        {
            if(Status == SurveyInstanceStatus.Submitted || Status == SurveyInstanceStatus.Expired)
            {
                throw new BusinessException("SurveyInstance.CannotMoveToInProgressFromFinalState")
                    .WithData("CurrentStatus", Status.ToString());
            }
            Status = SurveyInstanceStatus.InProgress;
            return this;
        }

        public SurveyInstance MarkSubmitted()
        {
            if(Status == SurveyInstanceStatus.Expired)
            {
                throw new BusinessException("SurveyInstance.CannotSubmitExpiredInstance");
            }
            Status = SurveyInstanceStatus.Submitted;
            return this;
        }

        public SurveyInstance MarkExpired()
        {
            if(Status == SurveyInstanceStatus.Submitted)
            {
                throw new BusinessException("SurveyInstance.CannotExpireSubmittedInstance");
            }
            Status = SurveyInstanceStatus.Expired;
            return this;
        }
            
    }
}
