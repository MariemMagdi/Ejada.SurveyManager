using Ejada.SurveyManager.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ejada.SurveyManager.Indicators
{
    public class Indicator : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Guid? CreatedByUserId => CreatorId;

        private Indicator() { } // EF Core

        private Indicator(Guid id, string name, string? description, bool isActive) : base(id)
        {
            SetName(name);
            SetDescription(description);
            if (isActive) Activate();
            else Deactivate();
        }

        public static Indicator Create(Guid id, string name, string? description = null, bool isActive = true)
            => new Indicator(id, name, description, isActive);

        public Indicator SetName(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            if (name.Length > DomainConstants.IndicatorNameMaxLength)
            {
                throw new BusinessException("Indicator.Name.MaxLength")
                    .WithData("Max", DomainConstants.IndicatorNameMaxLength);
            }
            Name = name.Trim();
            return this;
        }

        public Indicator SetDescription(string? description)
        {
            if (!string.IsNullOrWhiteSpace(description) && description.Length > DomainConstants.IndicatorDescriptionMaxLength)
            {
                throw new BusinessException("Indicator.Description.MaxLength")
                    .WithData("Max", DomainConstants.IndicatorDescriptionMaxLength);
            }
            Description = description?.Trim();
            return this;
        }

        public Indicator Activate()
        {
            IsActive = true;
            return this;
        }

        public Indicator Deactivate()
        {
            IsActive = false;
            return this;
        }
    }
}

