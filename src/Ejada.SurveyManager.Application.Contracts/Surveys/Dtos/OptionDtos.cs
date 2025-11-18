using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Ejada.SurveyManager.Surveys.Dtos
{
    public class OptionDto : EntityDto<Guid>
    {
        public Guid QuestionId { get;  set; }
        public string Label { get; set; } = string.Empty;
        public OptionDataType Type { get;  set; }
    }

    public class CreateOptionDto
    {
        public string Label { get; set; } = string.Empty;
        public OptionDataType Type { get; set; } = OptionDataType.String;
    }

    public class UpdateOptionDto : EntityDto<Guid>
    {
        public string Label { get; set; } = string.Empty;
        public OptionDataType Type { get; set; } = OptionDataType.String;
    }
}
