using Volo.Abp.Modularity;

namespace Ejada.SurveyManager;

[DependsOn(
    typeof(SurveyManagerApplicationModule),
    typeof(SurveyManagerDomainTestModule)
)]
public class SurveyManagerApplicationTestModule : AbpModule
{

}
