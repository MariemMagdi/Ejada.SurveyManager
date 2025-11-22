using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ejada.SurveyManager.Indicators;
using Ejada.SurveyManager.SurveyInstances;
using Ejada.SurveyManager.SurveyInstances.Enums;
using Ejada.SurveyManager.Surveys;
using Ejada.SurveyManager.Surveys.Enums;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace Ejada.SurveyManager.Data
{
    public class SurveyManagerTestDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Survey, Guid> _surveyRepository;
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IRepository<Option, Guid> _optionRepository;
        private readonly IRepository<SurveyQuestion, Guid> _surveyQuestionRepository;
        private readonly IRepository<SurveyInstance, Guid> _surveyInstanceRepository;
        private readonly IRepository<Response, Guid> _responseRepository;
        private readonly IRepository<ResponseOption, Guid> _responseOptionRepository;
        private readonly IRepository<Indicator, Guid> _indicatorRepository;
        private readonly IRepository<QuestionIndicator> _questionIndicatorRepository;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<SurveyManagerTestDataSeedContributor> _logger;

        // Fixed GUIDs for stable test data
        public static class TestDataIds
        {
            // Users
            public static readonly Guid AdminUserId = Guid.Parse("2e05cfcd-3a21-fe01-90ba-3a1d846ca715");
            public static readonly Guid Employee1Id = Guid.Parse("3a15cfcd-4b32-0e12-a1cb-4b2e957db826");
            public static readonly Guid Employee2Id = Guid.Parse("4b25cfcd-5c43-1f23-b2dc-5c3f068ec937");
            public static readonly Guid AuditorId = Guid.Parse("5c35cfcd-6d54-2034-c3ed-6d40179fd048");

            // Surveys
            public static readonly Guid Survey1Id = Guid.Parse("10000000-0000-0000-0000-000000000001");
            public static readonly Guid Survey2Id = Guid.Parse("10000000-0000-0000-0000-000000000002");
            public static readonly Guid Survey3Id = Guid.Parse("10000000-0000-0000-0000-000000000003");
            public static readonly Guid Survey4Id = Guid.Parse("10000000-0000-0000-0000-000000000004");
            public static readonly Guid Survey5Id = Guid.Parse("10000000-0000-0000-0000-000000000005");
            public static readonly Guid Survey6Id = Guid.Parse("10000000-0000-0000-0000-000000000006");
            public static readonly Guid Survey7Id = Guid.Parse("10000000-0000-0000-0000-000000000007");
            public static readonly Guid Survey8Id = Guid.Parse("10000000-0000-0000-0000-000000000008");

            // Questions
            public static readonly Guid Question1Id = Guid.Parse("20000000-0000-0000-0000-000000000001");
            public static readonly Guid Question2Id = Guid.Parse("20000000-0000-0000-0000-000000000002");
            public static readonly Guid Question3Id = Guid.Parse("20000000-0000-0000-0000-000000000003");
            public static readonly Guid Question4Id = Guid.Parse("20000000-0000-0000-0000-000000000004");
            public static readonly Guid Question5Id = Guid.Parse("20000000-0000-0000-0000-000000000005");
            public static readonly Guid Question6Id = Guid.Parse("20000000-0000-0000-0000-000000000006");
            public static readonly Guid Question7Id = Guid.Parse("20000000-0000-0000-0000-000000000007");
            public static readonly Guid Question8Id = Guid.Parse("20000000-0000-0000-0000-000000000008");
            public static readonly Guid Question9Id = Guid.Parse("20000000-0000-0000-0000-000000000009");
            public static readonly Guid Question10Id = Guid.Parse("20000000-0000-0000-0000-000000000010");

            // Indicators
            public static readonly Guid Indicator1Id = Guid.Parse("30000000-0000-0000-0000-000000000001");
            public static readonly Guid Indicator2Id = Guid.Parse("30000000-0000-0000-0000-000000000002");
            public static readonly Guid Indicator3Id = Guid.Parse("30000000-0000-0000-0000-000000000003");
            public static readonly Guid Indicator4Id = Guid.Parse("30000000-0000-0000-0000-000000000004");

            // Survey Instances
            public static readonly Guid Instance1Id = Guid.Parse("40000000-0000-0000-0000-000000000001");
            public static readonly Guid Instance2Id = Guid.Parse("40000000-0000-0000-0000-000000000002");
            public static readonly Guid Instance3Id = Guid.Parse("40000000-0000-0000-0000-000000000003");
            public static readonly Guid Instance4Id = Guid.Parse("40000000-0000-0000-0000-000000000004");
            public static readonly Guid Instance5Id = Guid.Parse("40000000-0000-0000-0000-000000000005");
            public static readonly Guid Instance6Id = Guid.Parse("40000000-0000-0000-0000-000000000006");
            public static readonly Guid Instance7Id = Guid.Parse("40000000-0000-0000-0000-000000000007");
        }

        public SurveyManagerTestDataSeedContributor(
            IRepository<Survey, Guid> surveyRepository,
            IRepository<Question, Guid> questionRepository,
            IRepository<Option, Guid> optionRepository,
            IRepository<SurveyQuestion, Guid> surveyQuestionRepository,
            IRepository<SurveyInstance, Guid> surveyInstanceRepository,
            IRepository<Response, Guid> responseRepository,
            IRepository<ResponseOption, Guid> responseOptionRepository,
            IRepository<Indicator, Guid> indicatorRepository,
            IRepository<QuestionIndicator> questionIndicatorRepository,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IdentityUserManager userManager,
            IGuidGenerator guidGenerator,
            ILogger<SurveyManagerTestDataSeedContributor> logger)
        {
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _optionRepository = optionRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _surveyInstanceRepository = surveyInstanceRepository;
            _responseRepository = responseRepository;
            _responseOptionRepository = responseOptionRepository;
            _indicatorRepository = indicatorRepository;
            _questionIndicatorRepository = questionIndicatorRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userManager = userManager;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            // Check if test data already exists
            if (await _surveyRepository.AnyAsync(s => s.Id == TestDataIds.Survey1Id))
            {
                _logger.LogInformation("Test data already seeded. Skipping...");
                return;
            }

            _logger.LogInformation("Seeding test data...");

            await SeedUsersAsync();
            await SeedIndicatorsAsync();
            await SeedQuestionsAsync();
            await SeedSurveysAsync();
            await SeedSurveyInstancesAsync();
            await SeedResponsesAsync();

            _logger.LogInformation("Test data seeding completed successfully.");
        }

        private async Task SeedUsersAsync()
        {
            _logger.LogInformation("Seeding users...");

            var employeeRole = await _roleRepository.FindByNormalizedNameAsync("EMPLOYEE");
            var auditorRole = await _roleRepository.FindByNormalizedNameAsync("AUDITOR");

            // Employee 1: John Smith
            if (await _userRepository.FindByNormalizedEmailAsync("JOHN.SMITH@TEST.COM") == null)
            {
                var employee1 = new IdentityUser(
                    TestDataIds.Employee1Id,
                    "john.smith",
                    "john.smith@test.com",
                    tenantId: null
                );
                employee1.SetEmailConfirmed(true);
                await _userManager.CreateAsync(employee1, "1q2w3E*");
                if (employeeRole != null)
                {
                    await _userManager.AddToRoleAsync(employee1, employeeRole.Name);
                }
            }

            // Employee 2: Sarah Johnson
            if (await _userRepository.FindByNormalizedEmailAsync("SARAH.JOHNSON@TEST.COM") == null)
            {
                var employee2 = new IdentityUser(
                    TestDataIds.Employee2Id,
                    "sarah.johnson",
                    "sarah.johnson@test.com",
                    tenantId: null
                );
                employee2.SetEmailConfirmed(true);
                await _userManager.CreateAsync(employee2, "1q2w3E*");
                if (employeeRole != null)
                {
                    await _userManager.AddToRoleAsync(employee2, employeeRole.Name);
                }
            }

            // Auditor: Mike Wilson
            if (await _userRepository.FindByNormalizedEmailAsync("MIKE.WILSON@TEST.COM") == null)
            {
                var auditor = new IdentityUser(
                    TestDataIds.AuditorId,
                    "mike.wilson",
                    "mike.wilson@test.com",
                    tenantId: null
                );
                auditor.SetEmailConfirmed(true);
                await _userManager.CreateAsync(auditor, "1q2w3E*");
                if (auditorRole != null)
                {
                    await _userManager.AddToRoleAsync(auditor, auditorRole.Name);
                }
            }
        }

        private async Task SeedIndicatorsAsync()
        {
            _logger.LogInformation("Seeding indicators...");

            var indicators = new[]
            {
                Indicator.Create(TestDataIds.Indicator1Id, "Employee Satisfaction", "Measures overall employee satisfaction and engagement", true),
                Indicator.Create(TestDataIds.Indicator2Id, "Work Environment", "Evaluates workplace conditions and culture", true),
                Indicator.Create(TestDataIds.Indicator3Id, "Training Effectiveness", "Assesses the quality and impact of training programs", true),
                Indicator.Create(TestDataIds.Indicator4Id, "Communication Quality", "Measures effectiveness of internal communication", true)
            };

            foreach (var indicator in indicators)
            {
                if (!await _indicatorRepository.AnyAsync(i => i.Id == indicator.Id))
                {
                    await _indicatorRepository.InsertAsync(indicator, autoSave: true);
                }
            }
        }

        private async Task SeedQuestionsAsync()
        {
            _logger.LogInformation("Seeding questions...");

            // Question 1: Likert 1-5 (Satisfaction)
            var q1 = Question.Create(TestDataIds.Question1Id, "How satisfied are you with your current role?", QuestionType.Likert1To5);
            await _questionRepository.InsertAsync(q1, autoSave: true);

            // Question 2: Likert 1-7 (Work-Life Balance)
            var q2 = Question.Create(TestDataIds.Question2Id, "Rate your work-life balance", QuestionType.Likert1To7);
            await _questionRepository.InsertAsync(q2, autoSave: true);

            // Question 3: Single Choice (Department)
            var q3 = Question.Create(TestDataIds.Question3Id, "Which department do you work in?", QuestionType.SingleChoice);
            await _questionRepository.InsertAsync(q3, autoSave: true);
            var q3Options = new[]
            {
                Option.Create(_guidGenerator.Create(), q3.Id, "IT", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q3.Id, "HR", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q3.Id, "Finance", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q3.Id, "Operations", OptionDataType.String)
            };
            await _optionRepository.InsertManyAsync(q3Options, autoSave: true);

            // Question 4: Multi Choice (Benefits)
            var q4 = Question.Create(TestDataIds.Question4Id, "Which benefits are most important to you? (Select all that apply)", QuestionType.MultiChoice);
            await _questionRepository.InsertAsync(q4, autoSave: true);
            var q4Options = new[]
            {
                Option.Create(_guidGenerator.Create(), q4.Id, "Health Insurance", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q4.Id, "Retirement Plan", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q4.Id, "Flexible Hours", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q4.Id, "Remote Work", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q4.Id, "Professional Development", OptionDataType.String)
            };
            await _optionRepository.InsertManyAsync(q4Options, autoSave: true);

            // Question 5: Likert 1-5 (Management)
            var q5 = Question.Create(TestDataIds.Question5Id, "How would you rate your direct manager's support?", QuestionType.Likert1To5);
            await _questionRepository.InsertAsync(q5, autoSave: true);

            // Question 6: Single Choice (Training Frequency)
            var q6 = Question.Create(TestDataIds.Question6Id, "How often do you receive training?", QuestionType.SingleChoice);
            await _questionRepository.InsertAsync(q6, autoSave: true);
            var q6Options = new[]
            {
                Option.Create(_guidGenerator.Create(), q6.Id, "Weekly", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q6.Id, "Monthly", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q6.Id, "Quarterly", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q6.Id, "Annually", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q6.Id, "Never", OptionDataType.String)
            };
            await _optionRepository.InsertManyAsync(q6Options, autoSave: true);

            // Question 7: Likert 1-7 (Communication)
            var q7 = Question.Create(TestDataIds.Question7Id, "Rate the effectiveness of internal communication", QuestionType.Likert1To7);
            await _questionRepository.InsertAsync(q7, autoSave: true);

            // Question 8: Multi Choice (Improvements)
            var q8 = Question.Create(TestDataIds.Question8Id, "What areas need improvement? (Select all that apply)", QuestionType.MultiChoice);
            await _questionRepository.InsertAsync(q8, autoSave: true);
            var q8Options = new[]
            {
                Option.Create(_guidGenerator.Create(), q8.Id, "Office Facilities", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q8.Id, "Technology Tools", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q8.Id, "Team Collaboration", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q8.Id, "Career Growth Opportunities", OptionDataType.String)
            };
            await _optionRepository.InsertManyAsync(q8Options, autoSave: true);

            // Question 9: Likert 1-5 (Workload)
            var q9 = Question.Create(TestDataIds.Question9Id, "Is your workload manageable?", QuestionType.Likert1To5);
            await _questionRepository.InsertAsync(q9, autoSave: true);

            // Question 10: Single Choice (Recommendation)
            var q10 = Question.Create(TestDataIds.Question10Id, "Would you recommend this company to others?", QuestionType.SingleChoice);
            await _questionRepository.InsertAsync(q10, autoSave: true);
            var q10Options = new[]
            {
                Option.Create(_guidGenerator.Create(), q10.Id, "Definitely Yes", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q10.Id, "Probably Yes", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q10.Id, "Not Sure", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q10.Id, "Probably No", OptionDataType.String),
                Option.Create(_guidGenerator.Create(), q10.Id, "Definitely No", OptionDataType.String)
            };
            await _optionRepository.InsertManyAsync(q10Options, autoSave: true);

            // Link indicators to questions
            var questionIndicators = new[]
            {
                new QuestionIndicator(TestDataIds.Question1Id, TestDataIds.Indicator1Id),
                new QuestionIndicator(TestDataIds.Question2Id, TestDataIds.Indicator1Id),
                new QuestionIndicator(TestDataIds.Question2Id, TestDataIds.Indicator2Id),
                new QuestionIndicator(TestDataIds.Question5Id, TestDataIds.Indicator1Id),
                new QuestionIndicator(TestDataIds.Question6Id, TestDataIds.Indicator3Id),
                new QuestionIndicator(TestDataIds.Question7Id, TestDataIds.Indicator4Id),
                new QuestionIndicator(TestDataIds.Question8Id, TestDataIds.Indicator2Id),
                new QuestionIndicator(TestDataIds.Question9Id, TestDataIds.Indicator2Id)
                // Note: Question 3, 4, and 10 have no indicators
            };
            await _questionIndicatorRepository.InsertManyAsync(questionIndicators, autoSave: true);
        }

        private async Task SeedSurveysAsync()
        {
            _logger.LogInformation("Seeding surveys...");

            // Survey 1: Annual Employee Satisfaction Survey (Active, with questions)
            var survey1 = Survey.Create(TestDataIds.Survey1Id, "Annual Employee Satisfaction Survey 2024", 
                "Comprehensive annual survey to measure employee satisfaction and engagement", 
                "All Employees");
            survey1.Activate();
            await _surveyRepository.InsertAsync(survey1, autoSave: true);
            await LinkQuestionsToSurvey(survey1.Id, new[] { TestDataIds.Question1Id, TestDataIds.Question2Id, TestDataIds.Question5Id, TestDataIds.Question9Id });

            // Survey 2: Quarterly Feedback Survey (Active, with questions)
            var survey2 = Survey.Create(TestDataIds.Survey2Id, "Q1 2024 Feedback Survey",
                "Quick quarterly check-in on employee experience",
                "All Departments");
            survey2.Activate();
            await _surveyRepository.InsertAsync(survey2, autoSave: true);
            await LinkQuestionsToSurvey(survey2.Id, new[] { TestDataIds.Question1Id, TestDataIds.Question7Id });

            // Survey 3: Training Effectiveness Survey (Active, with questions)
            var survey3 = Survey.Create(TestDataIds.Survey3Id, "Training Program Evaluation",
                "Evaluate the effectiveness of recent training programs",
                "Recently Trained Employees");
            survey3.Activate();
            await _surveyRepository.InsertAsync(survey3, autoSave: true);
            await LinkQuestionsToSurvey(survey3.Id, new[] { TestDataIds.Question6Id, TestDataIds.Question8Id });

            // Survey 4: Department Survey (Active, with questions)
            var survey4 = Survey.Create(TestDataIds.Survey4Id, "Department Information Survey",
                "Collect basic department information",
                "New Hires");
            survey4.Activate();
            await _surveyRepository.InsertAsync(survey4, autoSave: true);
            await LinkQuestionsToSurvey(survey4.Id, new[] { TestDataIds.Question3Id, TestDataIds.Question4Id });

            // Survey 5: Comprehensive Benefits Survey (Active, with many questions)
            var survey5 = Survey.Create(TestDataIds.Survey5Id, "Benefits and Workplace Survey",
                "Comprehensive survey covering benefits, workplace, and satisfaction",
                "Full-time Employees");
            survey5.Activate();
            await _surveyRepository.InsertAsync(survey5, autoSave: true);
            await LinkQuestionsToSurvey(survey5.Id, new[] { 
                TestDataIds.Question1Id, TestDataIds.Question2Id, TestDataIds.Question3Id, 
                TestDataIds.Question4Id, TestDataIds.Question5Id, TestDataIds.Question10Id 
            });

            // Survey 6: Empty Survey (Active, NO questions - for testing)
            var survey6 = Survey.Create(TestDataIds.Survey6Id, "Future Survey Template",
                "Survey template to be populated with questions later",
                "TBD");
            survey6.Activate();
            await _surveyRepository.InsertAsync(survey6, autoSave: true);
            // No questions linked

            // Survey 7: Inactive Survey (Inactive, with questions)
            var survey7 = Survey.Create(TestDataIds.Survey7Id, "Archived 2023 Survey",
                "Old survey from previous year - now inactive",
                "All Employees");
            survey7.Deactivate();
            await _surveyRepository.InsertAsync(survey7, autoSave: true);
            await LinkQuestionsToSurvey(survey7.Id, new[] { TestDataIds.Question1Id, TestDataIds.Question2Id });

            // Survey 8: Exit Interview Survey (Active, with questions)
            var survey8 = Survey.Create(TestDataIds.Survey8Id, "Exit Interview Survey",
                "Survey for employees leaving the organization",
                "Departing Employees");
            survey8.Activate();
            await _surveyRepository.InsertAsync(survey8, autoSave: true);
            await LinkQuestionsToSurvey(survey8.Id, new[] { TestDataIds.Question1Id, TestDataIds.Question7Id, TestDataIds.Question8Id, TestDataIds.Question10Id });
        }

        private async Task LinkQuestionsToSurvey(Guid surveyId, Guid[] questionIds)
        {
            var surveyQuestions = questionIds.Select(qId => new SurveyQuestion(
                _guidGenerator.Create(),
                surveyId,
                qId
            )).ToList();
            await _surveyQuestionRepository.InsertManyAsync(surveyQuestions, autoSave: true);
        }

        private async Task SeedSurveyInstancesAsync()
        {
            _logger.LogInformation("Seeding survey instances...");

            var now = DateTime.UtcNow;

            // Instance 1: Submitted (Employee 1, Survey 1) - Fully answered
            // Create with future date first to pass validation, then update
            var instance1 = SurveyInstance.Create(TestDataIds.Instance1Id, TestDataIds.Survey1Id, TestDataIds.Employee1Id, now.AddDays(1));
            instance1.MarkSubmitted();
            await _surveyInstanceRepository.InsertAsync(instance1, autoSave: false);
            // Manually set past due date after creation (bypassing validation for test data)
            typeof(SurveyInstance).GetProperty("DueDate").SetValue(instance1, now.AddDays(-7));
            await _surveyInstanceRepository.UpdateAsync(instance1, autoSave: true);

            // Instance 2: In Progress (Employee 1, Survey 2) - Partially answered, Due Soon
            var instance2 = SurveyInstance.Create(TestDataIds.Instance2Id, TestDataIds.Survey2Id, TestDataIds.Employee1Id, now.AddHours(12));
            instance2.MarkInProgress();
            await _surveyInstanceRepository.InsertAsync(instance2, autoSave: true);

            // Instance 3: Assigned (Employee 1, Survey 3) - Not started, Future due date
            var instance3 = SurveyInstance.Create(TestDataIds.Instance3Id, TestDataIds.Survey3Id, TestDataIds.Employee1Id, now.AddDays(5));
            await _surveyInstanceRepository.InsertAsync(instance3, autoSave: true);

            // Instance 4: Expired (Employee 2, Survey 1) - Overdue, not answered
            // Create with future date first to pass validation, then update
            var instance4 = SurveyInstance.Create(TestDataIds.Instance4Id, TestDataIds.Survey1Id, TestDataIds.Employee2Id, now.AddDays(1));
            instance4.MarkExpired();
            await _surveyInstanceRepository.InsertAsync(instance4, autoSave: false);
            // Manually set past due date after creation (bypassing validation for test data)
            typeof(SurveyInstance).GetProperty("DueDate").SetValue(instance4, now.AddDays(-3));
            await _surveyInstanceRepository.UpdateAsync(instance4, autoSave: true);

            // Instance 5: Submitted (Employee 2, Survey 4) - Fully answered
            // Create with future date first to pass validation, then update
            var instance5 = SurveyInstance.Create(TestDataIds.Instance5Id, TestDataIds.Survey4Id, TestDataIds.Employee2Id, now.AddDays(1));
            instance5.MarkSubmitted();
            await _surveyInstanceRepository.InsertAsync(instance5, autoSave: false);
            // Manually set past due date after creation (bypassing validation for test data)
            typeof(SurveyInstance).GetProperty("DueDate").SetValue(instance5, now.AddDays(-1));
            await _surveyInstanceRepository.UpdateAsync(instance5, autoSave: true);

            // Instance 6: Assigned (Employee 2, Survey 5) - Not started, No due date
            var instance6 = SurveyInstance.Create(TestDataIds.Instance6Id, TestDataIds.Survey5Id, TestDataIds.Employee2Id, null);
            await _surveyInstanceRepository.InsertAsync(instance6, autoSave: true);

            // Instance 7: Assigned (Employee 1, Survey 6 - Empty Survey) - No questions to answer
            var instance7 = SurveyInstance.Create(TestDataIds.Instance7Id, TestDataIds.Survey6Id, TestDataIds.Employee1Id, now.AddDays(10));
            await _surveyInstanceRepository.InsertAsync(instance7, autoSave: true);
        }

        private async Task SeedResponsesAsync()
        {
            _logger.LogInformation("Seeding responses...");

            // Get options for responses
            var allOptions = await _optionRepository.GetListAsync();
            var q3Options = allOptions.Where(o => o.QuestionId == TestDataIds.Question3Id).ToList();
            var q4Options = allOptions.Where(o => o.QuestionId == TestDataIds.Question4Id).ToList();
            var q10Options = allOptions.Where(o => o.QuestionId == TestDataIds.Question10Id).ToList();

            // Instance 1 Responses (Employee 1, Survey 1) - Fully answered and submitted
            await CreateLikertResponse(TestDataIds.Instance1Id, TestDataIds.Question1Id, 4);
            await CreateLikertResponse(TestDataIds.Instance1Id, TestDataIds.Question2Id, 5);
            await CreateLikertResponse(TestDataIds.Instance1Id, TestDataIds.Question5Id, 5);
            await CreateLikertResponse(TestDataIds.Instance1Id, TestDataIds.Question9Id, 3);

            // Instance 2 Responses (Employee 1, Survey 2) - Partially answered (in progress)
            await CreateLikertResponse(TestDataIds.Instance2Id, TestDataIds.Question1Id, 4);
            // Question 7 not answered yet

            // Instance 5 Responses (Employee 2, Survey 4) - Fully answered and submitted
            await CreateSingleChoiceResponse(TestDataIds.Instance5Id, TestDataIds.Question3Id, q3Options[0].Id); // IT
            await CreateMultiChoiceResponse(TestDataIds.Instance5Id, TestDataIds.Question4Id, 
                new[] { q4Options[0].Id, q4Options[2].Id, q4Options[4].Id }); // Health Insurance, Flexible Hours, Professional Development

            // Instance 3, 4, 6, 7 have no responses (not started or expired without answers)
        }

        private async Task CreateLikertResponse(Guid surveyInstanceId, Guid questionId, int value)
        {
            var response = Response.Create(_guidGenerator.Create(), surveyInstanceId, questionId);
            response.SetAnswerValue(value);
            await _responseRepository.InsertAsync(response, autoSave: true);
        }

        private async Task CreateSingleChoiceResponse(Guid surveyInstanceId, Guid questionId, Guid optionId)
        {
            var response = Response.Create(_guidGenerator.Create(), surveyInstanceId, questionId);
            await _responseRepository.InsertAsync(response, autoSave: true);
            
            var responseOption = ResponseOption.Create(_guidGenerator.Create(), response.Id, optionId);
            await _responseOptionRepository.InsertAsync(responseOption, autoSave: true);
        }

        private async Task CreateMultiChoiceResponse(Guid surveyInstanceId, Guid questionId, Guid[] optionIds)
        {
            var response = Response.Create(_guidGenerator.Create(), surveyInstanceId, questionId);
            await _responseRepository.InsertAsync(response, autoSave: true);
            
            foreach (var optionId in optionIds)
            {
                var responseOption = ResponseOption.Create(_guidGenerator.Create(), response.Id, optionId);
                await _responseOptionRepository.InsertAsync(responseOption, autoSave: true);
            }
        }
    }
}

