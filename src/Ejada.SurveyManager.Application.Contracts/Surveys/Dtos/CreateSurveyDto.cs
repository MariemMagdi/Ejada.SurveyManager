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
        [StringLength(1024)]
        public string Text { get; set; } = string.Empty;

        [Required]
        public QuestionType Type { get; set; }
        
        public List<CreateOptionInQuestionDto>? Options { get; set; }
    }
    
    public class CreateOptionInQuestionDto
    {
        [Required]
        [StringLength(256)]
        public string Label { get; set; } = string.Empty;
        
        public OptionDataType Type { get; set; } = OptionDataType.String;
    }
}
