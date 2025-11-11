using Ejada.SurveyManager.Localization;
using Volo.Abp.Application.Services;

namespace Ejada.SurveyManager;

/* Inherit your application services from this class.
 */
public abstract class SurveyManagerAppService : ApplicationService
{
    protected SurveyManagerAppService()
    {
        LocalizationResource = typeof(SurveyManagerResource);
    }
}
