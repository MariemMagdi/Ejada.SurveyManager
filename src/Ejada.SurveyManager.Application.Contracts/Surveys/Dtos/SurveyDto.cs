using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ejada.SurveyManager.Surveys.Dtos
{
    public class SurveyDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Purpose { get;  set; }
        public string? TargetAudience { get;  set; }
        public bool IsActive { get;  set; } 
        public Guid? CreatedByUserId { get; set; }
    }
}
