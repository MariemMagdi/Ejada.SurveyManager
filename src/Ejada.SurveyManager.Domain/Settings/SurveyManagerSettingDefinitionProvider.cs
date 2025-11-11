using Volo.Abp.Settings;

namespace Ejada.SurveyManager.Settings;

public class SurveyManagerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(SurveyManagerSettings.MySetting1));
    }
}
