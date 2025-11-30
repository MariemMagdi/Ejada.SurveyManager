using Ejada.SurveyManager.Indicators.Dtos;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ejada.SurveyManager.Indicators
{
    public interface IIndicatorAppService : ICrudAppService<
        IndicatorDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateIndicatorDto,
        UpdateIndicatorDto>
    {
        /// <summary>
        /// Exports indicator report as PDF
        /// </summary>
        Task<Stream> ExportIndicatorReportPdfAsync(Guid indicatorId, bool exportAllResponses = true);
    }
}

