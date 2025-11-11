using Volo.Abp.Modularity;

namespace Ejada.SurveyManager;

/* Inherit from this class for your domain layer tests. */
public abstract class SurveyManagerDomainTestBase<TStartupModule> : SurveyManagerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
