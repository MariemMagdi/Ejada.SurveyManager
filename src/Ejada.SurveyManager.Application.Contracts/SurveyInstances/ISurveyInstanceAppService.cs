using Ejada.SurveyManager.SurveyInstances.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ejada.SurveyManager.SurveyInstances
{
    public interface ISurveyInstanceAppService : ICrudAppService<
        SurveyInstanceDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateSurveyInstanceDto,
        UpdateSurveyInstanceDto>
    {
        Task<SurveyInstanceDto> MarkInProgressAsync(Guid id);
        Task<SurveyInstanceDto> SubmitAsync(Guid id);
        Task<SurveyInstanceDto> MarkExpiredAsync(Guid id);

    }
}
