using System.Text.RegularExpressions;

namespace Ejada.SurveyManager.EntityFrameworkCore.Helpers
{
    /// <summary>
    /// Helper class for database naming conventions.
    /// Converts PascalCase to Pascal_Snake_Case for table and column names.
    /// </summary>
    public static class NamingHelper
    {
        /// <summary>
        /// Converts a PascalCase string to Pascal_Snake_Case.
        /// Examples: 
        /// - "SurveyInstance" -> "Survey_Instance"
        /// - "AssigneeUserId" -> "Assignee_User_Id"
        /// - "IsActive" -> "Is_Active"
        /// </summary>
        public static string ToPascalSnakeCase(string pascalCaseString)
        {
            if (string.IsNullOrEmpty(pascalCaseString))
                return pascalCaseString;

            // Insert underscore before uppercase letters that follow lowercase letters
            // or before uppercase letters that are followed by lowercase letters
            var regex = new Regex("([a-z])([A-Z])|([A-Z])([A-Z][a-z])");
            var pascalSnakeCase = regex.Replace(pascalCaseString, "$1$3_$2$4");
            
            // Handle consecutive uppercase letters followed by lowercase
            pascalSnakeCase = Regex.Replace(pascalSnakeCase, "([A-Z]+)([A-Z][a-z])", "$1_$2");

            return pascalSnakeCase;
        }
    }
}

