namespace Ejada.SurveyManager.Permissions;

public static class SurveyManagerPermissions
{
    public const string GroupName = "SurveyManager";

    // Surveys
    public static class Surveys
    {
        public const string Default = GroupName + ".Surveys";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Questions
    public static class Questions
    {
        public const string Default = GroupName + ".Questions";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    // Survey Instances (Assignments)
    public static class SurveyInstances
    {
        public const string Default = GroupName + ".SurveyInstances";
        public const string ViewOwn = Default + ".ViewOwn";
        public const string ViewAll = Default + ".ViewAll";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string MarkExpired = Default + ".MarkExpired";
    }

    // Responses (Employee answering surveys)
    public static class Responses
    {
        public const string Default = GroupName + ".Responses";
        public const string ViewOwn = Default + ".ViewOwn";
        public const string ViewAll = Default + ".ViewAll";
        public const string Answer = Default + ".Answer";
        public const string Submit = Default + ".Submit";
    }

    // Indicators
    public static class Indicators
    {
        public const string Default = GroupName + ".Indicators";
        public const string ViewAll = Default + ".ViewAll";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
