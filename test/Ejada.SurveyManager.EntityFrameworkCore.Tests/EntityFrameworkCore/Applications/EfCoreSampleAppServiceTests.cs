using Ejada.SurveyManager.Samples;
using Xunit;

namespace Ejada.SurveyManager.EntityFrameworkCore.Applications;

[Collection(SurveyManagerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<SurveyManagerEntityFrameworkCoreTestModule>
{

}
