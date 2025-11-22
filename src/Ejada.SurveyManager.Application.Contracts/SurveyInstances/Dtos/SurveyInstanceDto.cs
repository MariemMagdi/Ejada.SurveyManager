//using Ejada.SurveyManager.SurveyInstances.Enums;
using Ejada.SurveyManager.SurveyInstances.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ejada.SurveyManager.SurveyInstances.Dtos
{
    public class SurveyInstanceDto : FullAuditedEntityDto<Guid>
    {
        public Guid SurveyId { get;  set; }
        public Guid AssigneeUserId { get;  set; }
        public DateTime? DueDate { get;  set; }
        public SurveyInstanceStatus Status { get;  set; }

        public string SurveyName { get; set; } = string.Empty;
    }

    public class CreateSurveyInstanceDto
    {
        [Required]
        public Guid SurveyId { get; set; }
        [Required]
        public Guid AssigneeUserId { get; set; }
        public DateTime? DueDate { get; set; }
    }

    public class UpdateSurveyInstanceDto
    {
        public DateTime? DueDate { get; set; }
    }
}
