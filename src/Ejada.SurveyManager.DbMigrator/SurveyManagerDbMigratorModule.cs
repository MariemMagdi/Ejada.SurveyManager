using Ejada.SurveyManager.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Ejada.SurveyManager.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(SurveyManagerEntityFrameworkCoreModule),
    typeof(SurveyManagerApplicationContractsModule)
)]
public class SurveyManagerDbMigratorModule : AbpModule
{
}
