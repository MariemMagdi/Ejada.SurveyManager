using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ejada.SurveyManager.Data;
using Volo.Abp.DependencyInjection;

namespace Ejada.SurveyManager.EntityFrameworkCore;

public class EntityFrameworkCoreSurveyManagerDbSchemaMigrator
    : ISurveyManagerDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreSurveyManagerDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the SurveyManagerDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<SurveyManagerDbContext>()
            .Database
            .MigrateAsync();
    }
}
