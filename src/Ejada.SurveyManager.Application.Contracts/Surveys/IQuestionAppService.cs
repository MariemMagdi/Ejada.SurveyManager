using Ejada.SurveyManager.Surveys.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ejada.SurveyManager.Surveys
{
    public interface IQuestionAppService : ICrudAppService<
        QuestionDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateQuestionDto, 
        UpdateQuestionDto>
    {
    }
}
