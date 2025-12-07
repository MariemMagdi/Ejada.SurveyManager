using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Ejada.SurveyManager.Blazor;

public class SurveyManagerStyleBundleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        // Load CSS files in priority order:
        // 1. Theme variables (must load first)
        context.Files.Add(new BundleFile("/css/ejada-theme.css", true));
        // 2. Main styles
        context.Files.Add(new BundleFile("main.css", true));
        // 3. Custom component overrides
        context.Files.Add(new BundleFile("/css/custom.css", true));
        // 4. Page-specific modern styles
        context.Files.Add(new BundleFile("/css/modernPageStyles.css", true));
    }
}
