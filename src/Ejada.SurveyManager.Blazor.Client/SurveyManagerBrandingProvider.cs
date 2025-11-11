using Microsoft.Extensions.Localization;
using Ejada.SurveyManager.Localization;
using Microsoft.Extensions.Localization;
using Ejada.SurveyManager.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Ejada.SurveyManager.Blazor.Client;

[Dependency(ReplaceServices = true)]
public class SurveyManagerBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<SurveyManagerResource> _localizer;

    public SurveyManagerBrandingProvider(IStringLocalizer<SurveyManagerResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
