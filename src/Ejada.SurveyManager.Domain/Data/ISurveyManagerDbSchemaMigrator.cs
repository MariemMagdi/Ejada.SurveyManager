using System.Threading.Tasks;

namespace Ejada.SurveyManager.Data;

public interface ISurveyManagerDbSchemaMigrator
{
    Task MigrateAsync();
}
