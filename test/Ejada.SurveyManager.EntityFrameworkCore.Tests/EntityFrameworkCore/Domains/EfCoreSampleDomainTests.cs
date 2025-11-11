using Ejada.SurveyManager.Samples;
using Xunit;

namespace Ejada.SurveyManager.EntityFrameworkCore.Domains;

[Collection(SurveyManagerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<SurveyManagerEntityFrameworkCoreTestModule>
{

}
