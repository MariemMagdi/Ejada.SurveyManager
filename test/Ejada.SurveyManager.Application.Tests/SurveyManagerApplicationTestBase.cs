using Volo.Abp.Modularity;

namespace Ejada.SurveyManager;

public abstract class SurveyManagerApplicationTestBase<TStartupModule> : SurveyManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
