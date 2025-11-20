using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Ejada.SurveyManager.Reporting.Dtos;

namespace Ejada.SurveyManager.Reporting
{
    public interface IAdminReportingAppService : IApplicationService
    {
        Task<PagedResultDto<SubmittedSurveyInstanceDto>> GetSubmittedSurveyInstancesAsync(GetSubmittedSurveyInstancesInput input);
        Task<SubmissionDetailDto> GetSubmissionDetailAsync(Guid surveyInstanceId);
    }
}
