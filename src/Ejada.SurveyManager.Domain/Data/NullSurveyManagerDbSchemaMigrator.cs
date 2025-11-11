using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Ejada.SurveyManager.Data;

/* This is used if database provider does't define
 * ISurveyManagerDbSchemaMigrator implementation.
 */
public class NullSurveyManagerDbSchemaMigrator : ISurveyManagerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
