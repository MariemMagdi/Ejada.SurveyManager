using Ejada.SurveyManager.Common;
using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ejada.SurveyManager.Surveys
{
    public class Survey : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string? Purpose { get; private set; }
        public string? TargetAudience { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Guid? CreatedByUserId => CreatorId;


        private Survey() { } //EF Core

        private Survey(Guid id, string name, string? purpose, string? targetAudience, bool isActive) : base(id)
        {
            SetName(name);
            SetPurpose(purpose);
            SetTargetAudience(targetAudience);
            if (isActive) Activate();
            else Deactivate();
        }

        public static Survey Create(Guid id, string name, string? purpose = null, string? targetAudience = null, bool isActive = true)
            => new Survey(id, name, purpose, targetAudience, isActive);
        public Survey SetName(string name)
        {
            Check.NotNullOrWhiteSpace(name,nameof(name));
            if(name.Length > DomainConstants.SurveyNameMaxLength)
            {
                throw new BusinessException("Survey.Name.MaxLength").WithData("Max", DomainConstants.SurveyNameMaxLength);
            }
            Name = name.Trim();
            return this;
        }

        public Survey SetPurpose(string purpose)
        {
            if (!string.IsNullOrWhiteSpace(purpose) && purpose.Length > DomainConstants.SurveyPurposeMaxLength)
            {
                throw new BusinessException("Survey.Purpose.MaxLength").WithData("Max", DomainConstants.SurveyPurposeMaxLength);
            }
            Purpose = string.IsNullOrWhiteSpace(purpose) ? null : purpose.Trim();
            return this;
        }

        public Survey SetTargetAudience(string targetAudience)
        {
            if (!string.IsNullOrWhiteSpace(targetAudience) && targetAudience.Length > DomainConstants.SurveyTargetAudienceMaxLength)
            {
                throw new BusinessException("Survey.TargetAudience.MaxLength").WithData("Max", DomainConstants.SurveyTargetAudienceMaxLength);
            }
            TargetAudience = string.IsNullOrWhiteSpace(targetAudience) ? null : targetAudience.Trim();
            return this;
        }

        public Survey Activate()
        {
            IsActive = true;
            return this;
        }

        public Survey Deactivate()
        {
            IsActive = false;
            return this;
        }
    }
}
