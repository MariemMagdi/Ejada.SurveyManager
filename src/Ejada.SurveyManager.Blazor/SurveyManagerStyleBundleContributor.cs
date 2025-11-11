using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Ejada.SurveyManager.Blazor;

public class SurveyManagerStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add(new BundleFile("main.css", true));
    }
}
