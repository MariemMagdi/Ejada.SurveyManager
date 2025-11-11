using Xunit;

namespace Ejada.SurveyManager.EntityFrameworkCore;

[CollectionDefinition(SurveyManagerTestConsts.CollectionDefinitionName)]
public class SurveyManagerEntityFrameworkCoreCollection : ICollectionFixture<SurveyManagerEntityFrameworkCoreFixture>
{

}
