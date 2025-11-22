using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Ejada.SurveyManager.Indicators.Dtos
{
    public class IndicatorDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Guid? CreatedByUserId { get; set; }
        
        // List of question IDs linked to this indicator
        public List<Guid> QuestionIds { get; set; } = new List<Guid>();
    }

    public class CreateIndicatorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        
        // List of question IDs to link to this indicator
        public List<Guid> QuestionIds { get; set; } = new List<Guid>();
    }

    public class UpdateIndicatorDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        
        // List of question IDs to link to this indicator
        public List<Guid> QuestionIds { get; set; } = new List<Guid>();
    }
}

