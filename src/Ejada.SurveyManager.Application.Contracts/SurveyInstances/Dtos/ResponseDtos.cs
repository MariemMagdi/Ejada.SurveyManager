using Ejada.SurveyManager.SurveyInstances.Enums;
using Ejada.SurveyManager.Surveys;
using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ejada.SurveyManager.SurveyInstances.Dtos
{
    public class ResponseDto : FullAuditedEntityDto<Guid>
    {
        public Guid SurveyInstanceId { get;  set; }
        public Guid QuestionId { get;  set; }
        public int? AnswerValue { get;  set; }
        public List<Guid> SelectedOptionIds { get; set; } = new();
    }

    public class SaveResponseDto
    {
        [Required]
        public Guid SurveyInstanceId { get; set; }
        [Required]
        public Guid QuestionId { get; set; }
        public int? AnswerValue { get; set; }
        public List<Guid>? SelectedOptionIds { get; set; }
    }

    public class SaveResponsesBulkDto
    {
        [Required]
        public Guid SurveyInstanceId { get; set; }
        [Required]
        [MinLength(1)]
        public List<SaveResponseDto> Responses { get; set; } = new();
    }

    public class SurveyInstanceForAnsweringDto : FullAuditedEntityDto<Guid>
    {
        public Guid SurveyId { get; set; }
        public Guid AssigneeUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public SurveyInstanceStatus Status { get; set; }
        public string? SurveyName { get; set; }
        public List<QuestionForAnsweringDto> Questions { get; set; } = new();
    }

    public class QuestionForAnsweringDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get; set; }

        public int? AnswerValue { get; set; }
        public List<Guid> SelectedOptionIds { get; set; } = new();

        public List<OptionForAnsweringDto> Options { get; set; } = new();
    }
    public class OptionForAnsweringDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; } = string.Empty;
        public OptionDataType Type { get; set; }
    }

}
