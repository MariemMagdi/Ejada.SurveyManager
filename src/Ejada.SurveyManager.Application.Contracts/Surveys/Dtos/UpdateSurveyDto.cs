using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.SurveyManager.Surveys.Dtos
{
    public class UpdateSurveyDto
    {
        [Required]
        [StringLength(256)]
        public string Name { get; set; } = string.Empty;

        [StringLength(512)]
        public string? Purpose { get; set; }

        [StringLength(256)]
        public string? TargetAudience { get; set; }
        public bool IsActive { get; set; } = true;

        public List<Guid>? AttachQuestionIds { get; set; }
        public List<Guid>? DetachQuestionIds { get; set; }

        // Create new questions inline while updating the survey
        public List<CreateQuestionInSurveyDto>? NewQuestions { get; set; }

        // Optional: update text/type of existing questions (use carefully)
        public List<UpdateQuestionInSurveyDto>? UpdateQuestions { get; set; }
    }

    public class UpdateQuestionInSurveyDto
    {
        [Required] 
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(1024)]
        public string Text { get; set; } = string.Empty;
        
        [Required] 
        public QuestionType Type { get; set; }
        
        public List<CreateOptionInQuestionDto>? Options { get; set; }
    }
}
