using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Ejada.SurveyManager.SurveyInstances.Enums;

namespace Ejada.SurveyManager.Reporting.Dtos
{
    public class SubmittedSurveyInstanceDto : EntityDto<Guid>
    {
        public Guid SurveyId { get; set; }
        public string? SurveyName { get; set; }
        public Guid AssigneeUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public SurveyInstanceStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class GetSubmittedSurveyInstancesInput : PagedAndSortedResultRequestDto
    {
        public Guid? SurveyId { get; set; }
        public Guid? AssigneeUserId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Search { get; set; } 
    }

    public class SubmittedResponseDetailDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public int? AnswerValue { get; set; }
        public List<Guid> SelectedOptionIds { get; set; } = new();
    }

    public class SubmissionDetailDto : EntityDto<Guid>
    {
        public Guid SurveyInstanceId { get; set; }
        public Guid SurveyId { get; set; }
        public string? SurveyName { get; set; }
        public Guid AssigneeUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public SurveyInstanceStatus Status { get; set; }
        public DateTime CreationTime { get; set; }

        public List<SubmittedResponseDetailDto> Responses { get; set; } = new();
    }
}
