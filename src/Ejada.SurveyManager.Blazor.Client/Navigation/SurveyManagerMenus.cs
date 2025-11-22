namespace Ejada.SurveyManager.Blazor.Client.Navigation;

public class SurveyManagerMenus
{
    private const string Prefix = "SurveyManager";

    public const string Home = Prefix + ".Home";
    public const string Surveys = Prefix + ".Surveys";
    public const string Questions = Prefix + ".Questions";
    public const string Indicators = Prefix + ".Indicators";
    
    // Admin menus
    public const string SurveyInstances = Prefix + ".SurveyInstances";
    
    // Employee menus
    public const string MySurveys = Prefix + ".MySurveys";
    
    // Auditor menus (same as admin but read-only)
    public const string AuditSurveyInstances = Prefix + ".AuditSurveyInstances";
}