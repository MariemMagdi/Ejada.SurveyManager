using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ejada.SurveyManager.Surveys.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Ejada.SurveyManager.Surveys.Dtos
{
    public class CreateSurveyDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; } = string.Empty;

        [StringLength(512)]
        public string? Purpose { get; set; }

        [StringLength(256)]
        public string? TargetAudience { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Guid>? ExistingQuestionIds { get; set; }

        public List<CreateQuestionInSurveyDto>? NewQuestions { get; set; }
    }

    public class CreateQuestionInSurveyDto
    {
        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public QuestionType Type { get; set; }
    }
}
