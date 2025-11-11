using Ejada.SurveyManager.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Ejada.SurveyManager.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class SurveyManagerController : AbpControllerBase
{
    protected SurveyManagerController()
    {
        LocalizationResource = typeof(SurveyManagerResource);
    }
}
