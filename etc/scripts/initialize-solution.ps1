abp install-libs

cd src/Ejada.SurveyManager.DbMigrator && dotnet run && cd -

cd src/Ejada.SurveyManager.Blazor && dotnet dev-certs https -v -ep openiddict.pfx -p f09dab4e-4867-47a9-9885-cf4e5fedf184




exit 0