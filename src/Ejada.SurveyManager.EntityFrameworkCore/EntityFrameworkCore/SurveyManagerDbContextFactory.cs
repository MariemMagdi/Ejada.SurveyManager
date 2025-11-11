using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Ejada.SurveyManager.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class SurveyManagerDbContextFactory : IDesignTimeDbContextFactory<SurveyManagerDbContext>
{
    public SurveyManagerDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        
        SurveyManagerEfCoreEntityExtensionMappings.Configure();

        var builder = new DbContextOptionsBuilder<SurveyManagerDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));
        
        return new SurveyManagerDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Ejada.SurveyManager.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
