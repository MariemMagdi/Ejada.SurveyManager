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
    public interface ISurveyAppService 
        : ICrudAppService<
            SurveyDto, 
            Guid,
            PagedAndSortedResultRequestDto,
            CreateSurveyDto,
            UpdateSurveyDto>
    {
    }
}
