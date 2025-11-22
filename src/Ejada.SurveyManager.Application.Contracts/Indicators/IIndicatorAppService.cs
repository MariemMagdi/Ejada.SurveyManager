using Ejada.SurveyManager.Indicators.Dtos;
using System;
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
    }
}

