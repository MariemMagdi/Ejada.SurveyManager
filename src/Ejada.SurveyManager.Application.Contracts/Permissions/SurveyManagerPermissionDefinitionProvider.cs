using Ejada.SurveyManager.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Ejada.SurveyManager.Permissions;

public class SurveyManagerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var surveyManagerGroup = context.AddGroup(SurveyManagerPermissions.GroupName, L("Permission:SurveyManager"));

        // Surveys
        var surveysPermission = surveyManagerGroup.AddPermission(
            SurveyManagerPermissions.Surveys.Default, 
            L("Permission:Surveys"));
        surveysPermission.AddChild(
            SurveyManagerPermissions.Surveys.Create, 
            L("Permission:Surveys.Create"));
        surveysPermission.AddChild(
            SurveyManagerPermissions.Surveys.Edit, 
            L("Permission:Surveys.Edit"));
        surveysPermission.AddChild(
            SurveyManagerPermissions.Surveys.Delete, 
            L("Permission:Surveys.Delete"));

        // Questions
        var questionsPermission = surveyManagerGroup.AddPermission(
            SurveyManagerPermissions.Questions.Default, 
            L("Permission:Questions"));
        questionsPermission.AddChild(
            SurveyManagerPermissions.Questions.Create, 
            L("Permission:Questions.Create"));
        questionsPermission.AddChild(
            SurveyManagerPermissions.Questions.Edit, 
            L("Permission:Questions.Edit"));
        questionsPermission.AddChild(
            SurveyManagerPermissions.Questions.Delete, 
            L("Permission:Questions.Delete"));

        // Survey Instances (Assignments)
        var surveyInstancesPermission = surveyManagerGroup.AddPermission(
            SurveyManagerPermissions.SurveyInstances.Default, 
            L("Permission:SurveyInstances"));
        surveyInstancesPermission.AddChild(
            SurveyManagerPermissions.SurveyInstances.ViewOwn, 
            L("Permission:SurveyInstances.ViewOwn"));
        surveyInstancesPermission.AddChild(
            SurveyManagerPermissions.SurveyInstances.ViewAll, 
            L("Permission:SurveyInstances.ViewAll"));
        surveyInstancesPermission.AddChild(
            SurveyManagerPermissions.SurveyInstances.Create, 
            L("Permission:SurveyInstances.Create"));
        surveyInstancesPermission.AddChild(
            SurveyManagerPermissions.SurveyInstances.Edit, 
            L("Permission:SurveyInstances.Edit"));
        surveyInstancesPermission.AddChild(
            SurveyManagerPermissions.SurveyInstances.Delete, 
            L("Permission:SurveyInstances.Delete"));
        surveyInstancesPermission.AddChild(
            SurveyManagerPermissions.SurveyInstances.MarkExpired, 
            L("Permission:SurveyInstances.MarkExpired"));

        // Responses
        var responsesPermission = surveyManagerGroup.AddPermission(
            SurveyManagerPermissions.Responses.Default, 
            L("Permission:Responses"));
        responsesPermission.AddChild(
            SurveyManagerPermissions.Responses.ViewOwn, 
            L("Permission:Responses.ViewOwn"));
        responsesPermission.AddChild(
            SurveyManagerPermissions.Responses.ViewAll, 
            L("Permission:Responses.ViewAll"));
        responsesPermission.AddChild(
            SurveyManagerPermissions.Responses.Answer, 
            L("Permission:Responses.Answer"));
        responsesPermission.AddChild(
            SurveyManagerPermissions.Responses.Submit, 
            L("Permission:Responses.Submit"));

        // Indicators
        var indicatorsPermission = surveyManagerGroup.AddPermission(
            SurveyManagerPermissions.Indicators.Default, 
            L("Permission:Indicators"));
        indicatorsPermission.AddChild(
            SurveyManagerPermissions.Indicators.ViewAll, 
            L("Permission:Indicators.ViewAll"));
        indicatorsPermission.AddChild(
            SurveyManagerPermissions.Indicators.Create, 
            L("Permission:Indicators.Create"));
        indicatorsPermission.AddChild(
            SurveyManagerPermissions.Indicators.Edit, 
            L("Permission:Indicators.Edit"));
        indicatorsPermission.AddChild(
            SurveyManagerPermissions.Indicators.Delete, 
            L("Permission:Indicators.Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SurveyManagerResource>(name);
    }
}
