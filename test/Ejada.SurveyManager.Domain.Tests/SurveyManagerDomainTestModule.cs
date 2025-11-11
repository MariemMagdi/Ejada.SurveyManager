using Volo.Abp.Modularity;

namespace Ejada.SurveyManager;

[DependsOn(
    typeof(SurveyManagerDomainModule),
    typeof(SurveyManagerTestBaseModule)
)]
public class SurveyManagerDomainTestModule : AbpModule
{

}
