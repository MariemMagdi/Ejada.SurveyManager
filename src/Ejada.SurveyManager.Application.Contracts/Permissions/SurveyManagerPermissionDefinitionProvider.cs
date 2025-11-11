using Ejada.SurveyManager.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Ejada.SurveyManager.Permissions;

public class SurveyManagerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SurveyManagerPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(SurveyManagerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SurveyManagerResource>(name);
    }
}
