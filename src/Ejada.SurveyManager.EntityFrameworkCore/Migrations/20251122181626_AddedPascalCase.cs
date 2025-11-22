using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ejada.SurveyManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedPascalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestions_Questions_QuestionId",
                table: "SurveyQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestions_Surveys_SurveyId",
                table: "SurveyQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SurveyInstances",
                table: "SurveyInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Responses",
                table: "Responses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponseOptions",
                table: "ResponseOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestionIndicators",
                table: "QuestionIndicators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Options",
                table: "Options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Indicators",
                table: "Indicators");

            migrationBuilder.RenameTable(
                name: "Surveys",
                newName: "Survey");

            migrationBuilder.RenameTable(
                name: "SurveyQuestions",
                newName: "Survey_Question");

            migrationBuilder.RenameTable(
                name: "SurveyInstances",
                newName: "Survey_Instance");

            migrationBuilder.RenameTable(
                name: "Responses",
                newName: "Response");

            migrationBuilder.RenameTable(
                name: "ResponseOptions",
                newName: "Response_Option");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameTable(
                name: "QuestionIndicators",
                newName: "Question_Indicator");

            migrationBuilder.RenameTable(
                name: "Options",
                newName: "Option");

            migrationBuilder.RenameTable(
                name: "Indicators",
                newName: "Indicator");

            migrationBuilder.RenameColumn(
                name: "TargetAudience",
                table: "Survey",
                newName: "Target_Audience");

            migrationBuilder.RenameColumn(
                name: "LastModifierId",
                table: "Survey",
                newName: "Last_Modifier_Id");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Survey",
                newName: "Last_Modification_Time");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Survey",
                newName: "Is_Deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Survey",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "DeletionTime",
                table: "Survey",
                newName: "Deletion_Time");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Survey",
                newName: "Deleter_Id");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Survey",
                newName: "Creator_Id");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Survey",
                newName: "Creation_Time");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "Survey_Question",
                newName: "Survey_Id");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Survey_Question",
                newName: "Question_Id");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestions_SurveyId_QuestionId",
                table: "Survey_Question",
                newName: "IX_Survey_Question_Survey_Id_Question_Id");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyQuestions_QuestionId",
                table: "Survey_Question",
                newName: "IX_Survey_Question_Question_Id");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "Survey_Instance",
                newName: "Survey_Id");

            migrationBuilder.RenameColumn(
                name: "LastModifierId",
                table: "Survey_Instance",
                newName: "Last_Modifier_Id");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Survey_Instance",
                newName: "Last_Modification_Time");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Survey_Instance",
                newName: "Is_Deleted");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "Survey_Instance",
                newName: "Due_Date");

            migrationBuilder.RenameColumn(
                name: "DeletionTime",
                table: "Survey_Instance",
                newName: "Deletion_Time");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Survey_Instance",
                newName: "Deleter_Id");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Survey_Instance",
                newName: "Creator_Id");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Survey_Instance",
                newName: "Creation_Time");

            migrationBuilder.RenameColumn(
                name: "AssigneeUserId",
                table: "Survey_Instance",
                newName: "Assignee_User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyInstances_SurveyId",
                table: "Survey_Instance",
                newName: "IX_Survey_Instance_Survey_Id");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyInstances_AssigneeUserId",
                table: "Survey_Instance",
                newName: "IX_Survey_Instance_Assignee_User_Id");

            migrationBuilder.RenameColumn(
                name: "SurveyInstanceId",
                table: "Response",
                newName: "Survey_Instance_Id");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Response",
                newName: "Question_Id");

            migrationBuilder.RenameColumn(
                name: "LastModifierId",
                table: "Response",
                newName: "Last_Modifier_Id");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Response",
                newName: "Last_Modification_Time");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Response",
                newName: "Creator_Id");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Response",
                newName: "Creation_Time");

            migrationBuilder.RenameColumn(
                name: "AnswerValue",
                table: "Response",
                newName: "Answer_Value");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_SurveyInstanceId",
                table: "Response",
                newName: "IX_Response_Survey_Instance_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Responses_QuestionId",
                table: "Response",
                newName: "IX_Response_Question_Id");

            migrationBuilder.RenameColumn(
                name: "ResponseId",
                table: "Response_Option",
                newName: "Response_Id");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                table: "Response_Option",
                newName: "Option_Id");

            migrationBuilder.RenameColumn(
                name: "LastModifierId",
                table: "Response_Option",
                newName: "Last_Modifier_Id");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Response_Option",
                newName: "Last_Modification_Time");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Response_Option",
                newName: "Creator_Id");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Response_Option",
                newName: "Creation_Time");

            migrationBuilder.RenameIndex(
                name: "IX_ResponseOptions_ResponseId",
                table: "Response_Option",
                newName: "IX_Response_Option_Response_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ResponseOptions_OptionId",
                table: "Response_Option",
                newName: "IX_Response_Option_Option_Id");

            migrationBuilder.RenameColumn(
                name: "LastModifierId",
                table: "Question",
                newName: "Last_Modifier_Id");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Question",
                newName: "Last_Modification_Time");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Question",
                newName: "Is_Deleted");

            migrationBuilder.RenameColumn(
                name: "DeletionTime",
                table: "Question",
                newName: "Deletion_Time");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Question",
                newName: "Deleter_Id");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Question",
                newName: "Creator_Id");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Question",
                newName: "Creation_Time");

            migrationBuilder.RenameColumn(
                name: "IndicatorId",
                table: "Question_Indicator",
                newName: "Indicator_Id");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Question_Indicator",
                newName: "Question_Id");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionIndicators_QuestionId",
                table: "Question_Indicator",
                newName: "IX_Question_Indicator_Question_Id");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionIndicators_IndicatorId",
                table: "Question_Indicator",
                newName: "IX_Question_Indicator_Indicator_Id");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Option",
                newName: "Question_Id");

            migrationBuilder.RenameColumn(
                name: "LastModifierId",
                table: "Option",
                newName: "Last_Modifier_Id");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Option",
                newName: "Last_Modification_Time");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Option",
                newName: "Is_Deleted");

            migrationBuilder.RenameColumn(
                name: "DeletionTime",
                table: "Option",
                newName: "Deletion_Time");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Option",
                newName: "Deleter_Id");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Option",
                newName: "Creator_Id");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Option",
                newName: "Creation_Time");

            migrationBuilder.RenameIndex(
                name: "IX_Options_QuestionId",
                table: "Option",
                newName: "IX_Option_Question_Id");

            migrationBuilder.RenameColumn(
                name: "LastModifierId",
                table: "Indicator",
                newName: "Last_Modifier_Id");

            migrationBuilder.RenameColumn(
                name: "LastModificationTime",
                table: "Indicator",
                newName: "Last_Modification_Time");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Indicator",
                newName: "Is_Deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Indicator",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "DeletionTime",
                table: "Indicator",
                newName: "Deletion_Time");

            migrationBuilder.RenameColumn(
                name: "DeleterId",
                table: "Indicator",
                newName: "Deleter_Id");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Indicator",
                newName: "Creator_Id");

            migrationBuilder.RenameColumn(
                name: "CreationTime",
                table: "Indicator",
                newName: "Creation_Time");

            migrationBuilder.RenameIndex(
                name: "IX_Indicators_Name",
                table: "Indicator",
                newName: "IX_Indicator_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Survey",
                table: "Survey",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Survey_Question",
                table: "Survey_Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Survey_Instance",
                table: "Survey_Instance",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Response",
                table: "Response",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Response_Option",
                table: "Response_Option",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question_Indicator",
                table: "Question_Indicator",
                columns: new[] { "Question_Id", "Indicator_Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Option",
                table: "Option",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Indicator",
                table: "Indicator",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_Question_Question_Id",
                table: "Option",
                column: "Question_Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Survey_Question_Question_Question_Id",
                table: "Survey_Question",
                column: "Question_Id",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Survey_Question_Survey_Survey_Id",
                table: "Survey_Question",
                column: "Survey_Id",
                principalTable: "Survey",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_Question_Question_Id",
                table: "Option");

            migrationBuilder.DropForeignKey(
                name: "FK_Survey_Question_Question_Question_Id",
                table: "Survey_Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Survey_Question_Survey_Survey_Id",
                table: "Survey_Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Survey_Question",
                table: "Survey_Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Survey_Instance",
                table: "Survey_Instance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Survey",
                table: "Survey");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Response_Option",
                table: "Response_Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Response",
                table: "Response");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question_Indicator",
                table: "Question_Indicator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Option",
                table: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Indicator",
                table: "Indicator");

            migrationBuilder.RenameTable(
                name: "Survey_Question",
                newName: "SurveyQuestions");

            migrationBuilder.RenameTable(
                name: "Survey_Instance",
                newName: "SurveyInstances");

            migrationBuilder.RenameTable(
                name: "Survey",
                newName: "Surveys");

            migrationBuilder.RenameTable(
                name: "Response_Option",
                newName: "ResponseOptions");

            migrationBuilder.RenameTable(
                name: "Response",
                newName: "Responses");

            migrationBuilder.RenameTable(
                name: "Question_Indicator",
                newName: "QuestionIndicators");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameTable(
                name: "Option",
                newName: "Options");

            migrationBuilder.RenameTable(
                name: "Indicator",
                newName: "Indicators");

            migrationBuilder.RenameColumn(
                name: "Survey_Id",
                table: "SurveyQuestions",
                newName: "SurveyId");

            migrationBuilder.RenameColumn(
                name: "Question_Id",
                table: "SurveyQuestions",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Survey_Question_Survey_Id_Question_Id",
                table: "SurveyQuestions",
                newName: "IX_SurveyQuestions_SurveyId_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Survey_Question_Question_Id",
                table: "SurveyQuestions",
                newName: "IX_SurveyQuestions_QuestionId");

            migrationBuilder.RenameColumn(
                name: "Survey_Id",
                table: "SurveyInstances",
                newName: "SurveyId");

            migrationBuilder.RenameColumn(
                name: "Last_Modifier_Id",
                table: "SurveyInstances",
                newName: "LastModifierId");

            migrationBuilder.RenameColumn(
                name: "Last_Modification_Time",
                table: "SurveyInstances",
                newName: "LastModificationTime");

            migrationBuilder.RenameColumn(
                name: "Is_Deleted",
                table: "SurveyInstances",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Due_Date",
                table: "SurveyInstances",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "Deletion_Time",
                table: "SurveyInstances",
                newName: "DeletionTime");

            migrationBuilder.RenameColumn(
                name: "Deleter_Id",
                table: "SurveyInstances",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "Creator_Id",
                table: "SurveyInstances",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Creation_Time",
                table: "SurveyInstances",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "Assignee_User_Id",
                table: "SurveyInstances",
                newName: "AssigneeUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Survey_Instance_Survey_Id",
                table: "SurveyInstances",
                newName: "IX_SurveyInstances_SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_Survey_Instance_Assignee_User_Id",
                table: "SurveyInstances",
                newName: "IX_SurveyInstances_AssigneeUserId");

            migrationBuilder.RenameColumn(
                name: "Target_Audience",
                table: "Surveys",
                newName: "TargetAudience");

            migrationBuilder.RenameColumn(
                name: "Last_Modifier_Id",
                table: "Surveys",
                newName: "LastModifierId");

            migrationBuilder.RenameColumn(
                name: "Last_Modification_Time",
                table: "Surveys",
                newName: "LastModificationTime");

            migrationBuilder.RenameColumn(
                name: "Is_Deleted",
                table: "Surveys",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Surveys",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Deletion_Time",
                table: "Surveys",
                newName: "DeletionTime");

            migrationBuilder.RenameColumn(
                name: "Deleter_Id",
                table: "Surveys",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "Creator_Id",
                table: "Surveys",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Creation_Time",
                table: "Surveys",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "Response_Id",
                table: "ResponseOptions",
                newName: "ResponseId");

            migrationBuilder.RenameColumn(
                name: "Option_Id",
                table: "ResponseOptions",
                newName: "OptionId");

            migrationBuilder.RenameColumn(
                name: "Last_Modifier_Id",
                table: "ResponseOptions",
                newName: "LastModifierId");

            migrationBuilder.RenameColumn(
                name: "Last_Modification_Time",
                table: "ResponseOptions",
                newName: "LastModificationTime");

            migrationBuilder.RenameColumn(
                name: "Creator_Id",
                table: "ResponseOptions",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Creation_Time",
                table: "ResponseOptions",
                newName: "CreationTime");

            migrationBuilder.RenameIndex(
                name: "IX_Response_Option_Response_Id",
                table: "ResponseOptions",
                newName: "IX_ResponseOptions_ResponseId");

            migrationBuilder.RenameIndex(
                name: "IX_Response_Option_Option_Id",
                table: "ResponseOptions",
                newName: "IX_ResponseOptions_OptionId");

            migrationBuilder.RenameColumn(
                name: "Survey_Instance_Id",
                table: "Responses",
                newName: "SurveyInstanceId");

            migrationBuilder.RenameColumn(
                name: "Question_Id",
                table: "Responses",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "Last_Modifier_Id",
                table: "Responses",
                newName: "LastModifierId");

            migrationBuilder.RenameColumn(
                name: "Last_Modification_Time",
                table: "Responses",
                newName: "LastModificationTime");

            migrationBuilder.RenameColumn(
                name: "Creator_Id",
                table: "Responses",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Creation_Time",
                table: "Responses",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "Answer_Value",
                table: "Responses",
                newName: "AnswerValue");

            migrationBuilder.RenameIndex(
                name: "IX_Response_Survey_Instance_Id",
                table: "Responses",
                newName: "IX_Responses_SurveyInstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_Response_Question_Id",
                table: "Responses",
                newName: "IX_Responses_QuestionId");

            migrationBuilder.RenameColumn(
                name: "Indicator_Id",
                table: "QuestionIndicators",
                newName: "IndicatorId");

            migrationBuilder.RenameColumn(
                name: "Question_Id",
                table: "QuestionIndicators",
                newName: "QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_Indicator_Question_Id",
                table: "QuestionIndicators",
                newName: "IX_QuestionIndicators_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Question_Indicator_Indicator_Id",
                table: "QuestionIndicators",
                newName: "IX_QuestionIndicators_IndicatorId");

            migrationBuilder.RenameColumn(
                name: "Last_Modifier_Id",
                table: "Questions",
                newName: "LastModifierId");

            migrationBuilder.RenameColumn(
                name: "Last_Modification_Time",
                table: "Questions",
                newName: "LastModificationTime");

            migrationBuilder.RenameColumn(
                name: "Is_Deleted",
                table: "Questions",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Deletion_Time",
                table: "Questions",
                newName: "DeletionTime");

            migrationBuilder.RenameColumn(
                name: "Deleter_Id",
                table: "Questions",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "Creator_Id",
                table: "Questions",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Creation_Time",
                table: "Questions",
                newName: "CreationTime");

            migrationBuilder.RenameColumn(
                name: "Question_Id",
                table: "Options",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "Last_Modifier_Id",
                table: "Options",
                newName: "LastModifierId");

            migrationBuilder.RenameColumn(
                name: "Last_Modification_Time",
                table: "Options",
                newName: "LastModificationTime");

            migrationBuilder.RenameColumn(
                name: "Is_Deleted",
                table: "Options",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Deletion_Time",
                table: "Options",
                newName: "DeletionTime");

            migrationBuilder.RenameColumn(
                name: "Deleter_Id",
                table: "Options",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "Creator_Id",
                table: "Options",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Creation_Time",
                table: "Options",
                newName: "CreationTime");

            migrationBuilder.RenameIndex(
                name: "IX_Option_Question_Id",
                table: "Options",
                newName: "IX_Options_QuestionId");

            migrationBuilder.RenameColumn(
                name: "Last_Modifier_Id",
                table: "Indicators",
                newName: "LastModifierId");

            migrationBuilder.RenameColumn(
                name: "Last_Modification_Time",
                table: "Indicators",
                newName: "LastModificationTime");

            migrationBuilder.RenameColumn(
                name: "Is_Deleted",
                table: "Indicators",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Indicators",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Deletion_Time",
                table: "Indicators",
                newName: "DeletionTime");

            migrationBuilder.RenameColumn(
                name: "Deleter_Id",
                table: "Indicators",
                newName: "DeleterId");

            migrationBuilder.RenameColumn(
                name: "Creator_Id",
                table: "Indicators",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Creation_Time",
                table: "Indicators",
                newName: "CreationTime");

            migrationBuilder.RenameIndex(
                name: "IX_Indicator_Name",
                table: "Indicators",
                newName: "IX_Indicators_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyQuestions",
                table: "SurveyQuestions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SurveyInstances",
                table: "SurveyInstances",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponseOptions",
                table: "ResponseOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Responses",
                table: "Responses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestionIndicators",
                table: "QuestionIndicators",
                columns: new[] { "QuestionId", "IndicatorId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Options",
                table: "Options",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Indicators",
                table: "Indicators",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestions_Questions_QuestionId",
                table: "SurveyQuestions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestions_Surveys_SurveyId",
                table: "SurveyQuestions",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
