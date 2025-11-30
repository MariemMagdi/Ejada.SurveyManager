using Ejada.SurveyManager.SurveyInstances.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Ejada.SurveyManager.SurveyInstances
{
    public interface IResponseAppService : IApplicationService
    {
        /// <summary>
        /// Save multiple responses (draft or partial). Caller must be the assignee.
        /// Validates question types and ranges. Does NOT submit the instance.
        /// </summary>
        Task SaveResponsesBulkAsync(SaveResponsesBulkDto input);

        /// <summary>
        /// Get a single response by id (for admin/audit). Visibility rules apply.
        /// </summary>
        Task<ResponseDto> GetResponseAsync(Guid id);

        /// <summary>
        /// Get all responses for a survey instance (for assignee or authorized viewers).
        /// </summary>
        Task<List<ResponseDto>> GetResponsesForInstanceAsync(Guid surveyInstanceId);

        /// <summary>
        /// Returns the full read model needed to present the survey to the assignee,
        /// including questions, options and any existing answers.
        /// Visibility rules applied (e.g. admin/auditor cannot see answers until submitted).
        /// </summary>
        Task<SurveyInstanceForAnsweringDto> GetSurveyInstanceForAnsweringAsync(Guid surveyInstanceId);

        Task SubmitSurveyInstanceAsync(Guid surveyInstanceId);
        Task SaveAndSubmitResponsesBulkAsync(SaveResponsesBulkDto input);

        /// <summary>
        /// Get all responses for a specific question (for admin/auditor to view indicator statistics).
        /// Returns responses across all survey instances.
        /// </summary>
        Task<List<QuestionResponseSummaryDto>> GetResponsesByQuestionIdAsync(Guid questionId);

        /// <summary>
        /// Get detailed responses for a specific question with employee emails and formatted answers.
        /// Returns responses across all survey instances with employee information.
        /// </summary>
        Task<List<QuestionResponseDetailDto>> GetQuestionResponseDetailsAsync(Guid questionId);

    }
}
