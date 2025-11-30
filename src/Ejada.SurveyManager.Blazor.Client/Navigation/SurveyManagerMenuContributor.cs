using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ejada.SurveyManager.Localization;
using Ejada.SurveyManager.Permissions;
using Ejada.SurveyManager.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.Users;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.Authorization;

namespace Ejada.SurveyManager.Blazor.Client.Navigation;

public class SurveyManagerMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public SurveyManagerMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<SurveyManagerResource>();
        
        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        // Dashboard - visible to authenticated users
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.Dashboard,
            l["Menu:Dashboard"],
            "/dashboard",
            icon: "fas fa-chart-pie",
            order: 1
        ).RequireAuthenticated());

        // Home - visible to everyone
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.Home,
            l["Menu:Home"],
            "/",
            icon: "fas fa-home",
            order: 2
        ));

        // Surveys - Admin only (requires permission to view/manage surveys)
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.Surveys,
            l["Menu:Surveys"],
            "/surveys",
            icon: "fas fa-poll",
            order: 3,
            requiredPermissionName: SurveyManagerPermissions.Surveys.Default
        ));

        // Questions - Admin only (requires permission to view/manage questions)
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.Questions,
            l["Menu:Questions"],
            "/questions",
            icon: "fas fa-question-circle",
            order: 4,
            requiredPermissionName: SurveyManagerPermissions.Questions.Default
        ));

        // Indicators - Admin (Create/Edit/Delete) and Auditor (View only)
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.Indicators,
            l["Menu:Indicators"],
            "/indicators",
            icon: "fas fa-chart-line",
            order: 5,
            requiredPermissionName: SurveyManagerPermissions.Indicators.ViewAll
        ));

        // Survey Instances (Assignments) - Admin and Auditor (requires permission to view assignments)
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.SurveyInstances,
            l["Menu:SurveyInstances"],
            "/survey-instances",
            icon: "fas fa-clipboard-list",
            order: 6,
            requiredPermissionName: SurveyManagerPermissions.SurveyInstances.ViewAll
        ));

        // My Surveys - Employee (requires permission to view own assigned surveys)
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.MySurveys,
            l["Menu:MySurveys"],
            "/my-surveys",
            icon: "fas fa-tasks",
            order: 7,
            requiredPermissionName: SurveyManagerPermissions.SurveyInstances.ViewOwn
        ));

        // Users - Admin only (requires permission to view users)
        context.Menu.AddItem(new ApplicationMenuItem(
            SurveyManagerMenus.Users,
            l["Menu:Users"],
            "/users",
            icon: "fas fa-users",
            order: 8,
            requiredPermissionName: "AbpIdentity.Users"
        ));

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);
        
        await Task.CompletedTask;
    }

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
            icon: "fa fa-cog",
            order: 1000,
            target: "_blank").RequireAuthenticated());

        await Task.CompletedTask;
    }
}
