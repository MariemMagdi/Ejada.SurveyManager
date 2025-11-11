using Ejada.SurveyManager.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Ejada.SurveyManager.Blazor.Client;

public abstract class SurveyManagerComponentBase : AbpComponentBase
{
    protected SurveyManagerComponentBase()
    {
        LocalizationResource = typeof(SurveyManagerResource);
    }
}
