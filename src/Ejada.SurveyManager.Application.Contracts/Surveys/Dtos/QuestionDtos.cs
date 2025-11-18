using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ejada.SurveyManager.Surveys.Dtos
{
    public class QuestionDto : FullAuditedEntityDto<Guid>
    {
        public string Text { get; set; } = string.Empty;
        public QuestionType Type { get;  set; }
        public List<OptionDto> Options { get; set; } = new();
    }

    public class CreateQuestionDto
    {
        [Required]
        [StringLength(512)]
        public string Text { get; set; } = string.Empty;
        [Required]
        public QuestionType Type { get;  set; }
        public List<CreateOptionDto>? Options { get; set; } 
    }

    public class UpdateQuestionDto
    {
        [Required]
        [StringLength(512)]
        public string Text { get; set; } = string.Empty;
        [Required]
        public QuestionType Type { get; set; }
        public List<UpdateOptionDto>? Options { get; set; }
    }
}
