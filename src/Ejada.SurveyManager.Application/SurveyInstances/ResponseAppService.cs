using Ejada.SurveyManager.Permissions;
using Ejada.SurveyManager.SurveyInstances.Dtos;
using Ejada.SurveyManager.SurveyInstances.Enums;
using Ejada.SurveyManager.Surveys;
using Ejada.SurveyManager.Surveys.Dtos;
using Ejada.SurveyManager.Surveys.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Ejada.SurveyManager.SurveyInstances
{
    public class ResponseAppService : ApplicationService, IResponseAppService
    {
        private readonly IRepository<Survey, Guid> _surveyRepository;
        private readonly IRepository<SurveyInstance, Guid> _surveyInstanceRepository;
        private readonly IRepository<Response, Guid> _responseRepository;
        private readonly IRepository<Question, Guid> _questionRepository;
        private readonly IRepository<SurveyQuestion, Guid> _surveyQuestionRepository;
        private readonly IRepository<Option, Guid> _optionRepository;
        private readonly IRepository<ResponseOption, Guid> _responseOptionRepository;
        public ResponseAppService(
            IRepository<Survey, Guid> surveyRepository,
            IRepository<SurveyInstance, Guid> surveyInstanceRepository,
            IRepository<Response, Guid> responseRepository,
            IRepository<Response,Guid> repository,
            IRepository<Question,Guid> questionRepository,
            IRepository<SurveyQuestion, Guid> surveyQuestionRepository,
            IRepository<Option, Guid> optionRepository,
            IRepository<ResponseOption, Guid> responseOptionRepository)
        {
            _surveyRepository = surveyRepository;
            _surveyInstanceRepository = surveyInstanceRepository;
            _responseRepository = responseRepository;
            _questionRepository = questionRepository;
            _surveyQuestionRepository = surveyQuestionRepository;
            _optionRepository = optionRepository;
            _responseOptionRepository = responseOptionRepository;
        }

        [Authorize(SurveyManagerPermissions.Responses.Answer)]
        public async Task SaveResponsesBulkAsync(SaveResponsesBulkDto input)
        {
            //load instance 
            var instance = await _surveyInstanceRepository.GetAsync(input.SurveyInstanceId);

            // ensure caller is assignee & instance editable
            await EnsureAssigneeAndEditableAsync(instance);

            // mark instance as InProgress if it is still Assigned
            if(instance.Status == SurveyInstanceStatus.Assigned)
            {
                instance.MarkInProgress();
                await _surveyInstanceRepository.UpdateAsync(instance, autoSave: true);
            }

            // validate each response individually
            foreach(var dto in input.Responses)
            {
                await ValidateSaveResponseDtoAsync(dto);
            }

            // Upsert responses & response-options
            foreach(var dto in input.Responses)
            {
                var existingResponse = (await _responseRepository.GetListAsync(r =>
                r.SurveyInstanceId == dto.SurveyInstanceId && r.QuestionId == dto.QuestionId)).FirstOrDefault();

                if(existingResponse == null)
                {
                    var newResponse = Response.Create(
                        GuidGenerator.Create(),
                        dto.SurveyInstanceId,
                        dto.QuestionId,
                        dto.AnswerValue);

                    await _responseRepository.InsertAsync(newResponse, autoSave: true);

                    //Insert selected options if any
                    if(dto.SelectedOptionIds!=null && dto.SelectedOptionIds.Any())
                    {
                        var responseOptionEntites = dto.SelectedOptionIds
                            .Where(id => id != Guid.Empty)
                            .Distinct()
                            .Select(id =>
                            ResponseOption.Create(
                                GuidGenerator.Create(),
                                id,
                                newResponse.Id)).ToList();

                        if (responseOptionEntites.Any())
                        {
                            await _responseOptionRepository.InsertManyAsync(responseOptionEntites, autoSave: true);
                        }
                    }
                }
                else
                {
                    existingResponse.SetAnswerValue(dto.AnswerValue);
                    await _responseRepository.UpdateAsync(existingResponse,autoSave: true);

                    var existingResponseOptions = await _responseOptionRepository.GetListAsync(ro => ro.ResponseId == existingResponse.Id);
                    if(existingResponseOptions != null)
                    {
                        await _responseOptionRepository.DeleteManyAsync(existingResponseOptions, autoSave: true);
                    }
                    if (dto.SelectedOptionIds != null && dto.SelectedOptionIds.Any())
                    {
                        var newResponseOptions = dto.SelectedOptionIds
                            .Where(id => id != Guid.Empty)
                            .Distinct()
                            .Select(id =>
                            ResponseOption.Create(
                                GuidGenerator.Create(),
                                id,
                                existingResponse.Id)).ToList();

                        if (newResponseOptions.Any())
                        {
                            await _responseOptionRepository.InsertManyAsync(newResponseOptions, autoSave: true);
                        }
                    }
                }
            }
        }

        [Authorize(SurveyManagerPermissions.Responses.ViewAll)]
        public virtual async Task<List<ResponseDto>> GetResponsesForInstanceAsync(Guid surveyInstanceId) 
        {
            var instance = await _surveyInstanceRepository.GetAsync(surveyInstanceId);

            // Visibility check:
            // - If caller is the assignee -> allowed (for Assigned/InProgress/Submitted)
            // - If caller is NOT the assignee -> only allowed when instance is Submitted
            var callerId = CurrentUser.GetId();

            if(instance.AssigneeUserId != callerId && instance.Status != SurveyInstanceStatus.Submitted)
            {
                throw new BusinessException("SurveyInstance.View.NotAllowed")
                    .WithData("SurveyInstanceId", surveyInstanceId)
                    .WithData("CurrentUserId", callerId)
                    .WithData("Status", instance.Status.ToString());
            }

            var responses = await _responseRepository.GetListAsync(r => r.SurveyInstanceId == surveyInstanceId);
            if (!responses.Any())
                return new List<ResponseDto>();

            var responseIds = responses.Select(r=>r.Id).ToList();
            var responseOptions = await _responseOptionRepository.GetListAsync(ro => responseIds.Contains(ro.ResponseId));
            var optionsByResponse = responseOptions
                .GroupBy(ro => ro.ResponseId)
                .ToDictionary(g => g.Key, g => g.Select(x=>x.OptionId).ToList());

            var dtos = responses.Select(r => new ResponseDto
            {
                Id = r.Id,
                SurveyInstanceId = r.SurveyInstanceId,
                AnswerValue = r.AnswerValue,
                QuestionId = r.QuestionId,
                SelectedOptionIds = optionsByResponse.TryGetValue(r.Id, out var list) ? list : new List<Guid>()
            }).ToList();

            return dtos;
        }

        [Authorize(SurveyManagerPermissions.Responses.ViewAll)]
        public virtual async Task<ResponseDto> GetResponseAsync(Guid id) 
        {
            var response = await _responseRepository.GetAsync(id);

            var instance = await _surveyInstanceRepository.GetAsync(response.SurveyInstanceId);

            var callerId = CurrentUser.GetId();

            if(instance.AssigneeUserId != callerId && instance.Status != SurveyInstanceStatus.Submitted)
            {
                throw new BusinessException("Response.View.NotAllowed")
                    .WithData("ResponseId", id)
                    .WithData("SurveyInstanceId", instance.Id)
                    .WithData("CurrentUserId", callerId)
                    .WithData("Status", instance.Status.ToString());
            }

            var selectedOptions = await _responseOptionRepository.GetListAsync(ro => ro.ResponseId == id);
            var selectedOptionIds = selectedOptions.Select(o=>o.Id).ToList();

            var dto = new ResponseDto
            {
                Id = id,
                SurveyInstanceId = response.SurveyInstanceId,
                QuestionId = response.QuestionId,
                AnswerValue = response.AnswerValue,
                SelectedOptionIds = selectedOptionIds
            };
            return dto;
        }

        [Authorize(SurveyManagerPermissions.Responses.ViewOwn)]
        public virtual async Task<SurveyInstanceForAnsweringDto> GetSurveyInstanceForAnsweringAsync(Guid surveyInstanceId) 
        {
            var instance = await _surveyInstanceRepository.GetAsync(surveyInstanceId);
            var callerId = CurrentUser.GetId();

            if (instance.AssigneeUserId != callerId && instance.Status != SurveyInstanceStatus.Submitted)
            {
                throw new BusinessException("SurveyInstance.View.NotAllowed")
                    .WithData("SurveyInstanceId", surveyInstanceId)
                    .WithData("CurrentUserId", callerId)
                    .WithData("Status", instance.Status.ToString());
            }

            var survey = await _surveyRepository.GetAsync(s => s.Id == instance.SurveyId);

            //load question links for this survey
            var surveyQuestionLinks = await _surveyQuestionRepository.GetListAsync(sq => sq.SurveyId == instance.SurveyId);
            var questionIds = surveyQuestionLinks.Select(sq=>sq.QuestionId).ToList();

            var questions = questionIds.Any()
                ? await _questionRepository.GetListAsync(q => questionIds.Contains(q.Id))
                : new List<Question>();

            var questionById = questions.ToDictionary(q => q.Id, q => q);

            var options = questionIds.Any()
                ? await _optionRepository.GetListAsync(o=>questionIds.Contains(o.QuestionId))
                : new List<Option>();

            var optionsByQuestion = options
                .GroupBy(o=>o.QuestionId)
                .ToDictionary(g=>g.Key, g=> g.ToList());

            var existingResponses = await _responseRepository.GetListAsync(r => r.SurveyInstanceId == surveyInstanceId);
            var responseIds = existingResponses.Select(r=>r.Id).ToList();

            var responseOptions = responseIds.Any()
                ? await _responseOptionRepository.GetListAsync(ro => responseIds.Contains(ro.ResponseId))
                : new List<ResponseOption>();

            var selectedOptionsByResponse = responseOptions
                .GroupBy(ro=>ro.ResponseId)
                .ToDictionary(g=>g.Key, g=>g.Select(o=>o.OptionId).ToList());

            var responseByQuestion = existingResponses.ToDictionary(r=>r.QuestionId, r=> r);

            //Build Dto
            var result = new SurveyInstanceForAnsweringDto
            {
                Id = instance.Id,
                SurveyId = instance.SurveyId,
                AssigneeUserId = instance.AssigneeUserId,
                DueDate = instance.DueDate,
                Status = instance.Status,
                SurveyName = survey?.Name
            };

            foreach( var sq in surveyQuestionLinks)
            {
                if (!questionById.TryGetValue(sq.QuestionId, out var question)) continue;

                var qDto = new QuestionForAnsweringDto
                {
                    Id = question.Id,
                    Text = question.Text,
                    Type = question.Type
                };

                if(optionsByQuestion.TryGetValue(question.Id, out var qOptions))
                {
                    qDto.Options = qOptions.Select(o => new OptionForAnsweringDto
                    {
                        Id = o.Id,
                        Label = o.Label,
                        Type = o.Type
                    }).ToList();
                }
                else
                {
                    qDto.Options = new List<OptionForAnsweringDto>();
                }

                if(responseByQuestion.TryGetValue(question.Id, out var response))
                {
                    qDto.AnswerValue = response.AnswerValue;
                    if(selectedOptionsByResponse.TryGetValue(response.Id, out var selectedOptionIds))
                    {
                        qDto.SelectedOptionIds = selectedOptionIds;
                    }
                }

                result.Questions.Add(qDto);
            }
            return result;
        }

        [Authorize(SurveyManagerPermissions.Responses.Submit)]
        public async Task SubmitSurveyInstanceAsync(Guid surveyInstanceId)
        {
            // 1) Load instance and quick checks
            var instance = await _surveyInstanceRepository.GetAsync(surveyInstanceId);
            var callerId = CurrentUser.GetId();

            if (instance.AssigneeUserId != callerId)
            {
                throw new BusinessException("SurveyInstance.Submit.NotAssignee")
                    .WithData("SurveyInstanceId", surveyInstanceId)
                    .WithData("CurrentUserId", callerId);
            }

            if (instance.Status == SurveyInstanceStatus.Submitted)
            {
                return;
            }

            if (instance.Status == SurveyInstanceStatus.Expired)
            {
                throw new BusinessException("SurveyInstance.Submit.Expired")
                    .WithData("SurveyInstanceId", surveyInstanceId);
            }

            if (instance.DueDate.HasValue && instance.DueDate.Value < DateTime.UtcNow)
            {
                instance.MarkExpired();
                await _surveyInstanceRepository.UpdateAsync(instance, autoSave: true);

                throw new BusinessException("SurveyInstance.Submit.DueDatePassed")
                    .WithData("SurveyInstanceId", surveyInstanceId)
                    .WithData("DueDate", instance.DueDate.Value);
            }

            // 2) Load all questions for this survey (use SurveyQuestion linking table)
            var surveyQuestionLinks = await _surveyQuestionRepository.GetListAsync(sq => sq.SurveyId == instance.SurveyId);
            var questionIds = surveyQuestionLinks.Select(sq=>sq.QuestionId).ToList();

            var questions = questionIds.Any()
                ? await _questionRepository.GetListAsync(q=>questionIds.Contains(q.Id))
                : new List<Question> ();
            var questionsById = questions.ToDictionary(q => q.Id, q => q);

            // 3) Load existing responses & response-options for this instance
            var existingResponses = await _responseRepository.GetListAsync(r => r.SurveyInstanceId == surveyInstanceId);
            var responseByQuestion = existingResponses.ToDictionary(r => r.QuestionId, r => r);

            var responseIds = existingResponses.Select(r=>r.Id).ToList();
            var responseOptions = responseIds.Any()
                ? await _responseOptionRepository.GetListAsync(ro => responseIds.Contains(ro.ResponseId))
                : new List<ResponseOption> ();
            var responseOptionsByResponse = responseOptions
                .GroupBy(ro => ro.ResponseId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.OptionId).ToList());

            // 4) Validate every question must satisfy its requirement at submit time
            foreach(var qId in questionIds)
            {
                // If question was removed from DB, skip (safety)
                if (!questionsById.TryGetValue(qId, out var question))
                    continue;

                responseByQuestion.TryGetValue(qId, out var response);

                // Build a DTO that the validator accepts
                var dto = new SaveResponseDto
                {
                    SurveyInstanceId = surveyInstanceId,
                    QuestionId = qId,
                    AnswerValue = response?.AnswerValue,
                    SelectedOptionIds = response == null
                        ? new List<Guid>()
                        : (responseOptionsByResponse.TryGetValue(response.Id, out var opts) ? opts : new List<Guid>())
                };

                await ValidateSaveResponseDtoAsync(dto);

                // --- Submit-time extra rules ---
                switch (question.Type)
                {
                    case QuestionType.Likert1To5:
                        if (dto.AnswerValue == null)
                            throw new BusinessException("Submit.Validation.Likert.Required")
                                .WithData("QuestionId", qId);
                        break;

                    case QuestionType.Likert1To7:
                        if (dto.AnswerValue == null)
                            throw new BusinessException("Submit.Validation.Likert.Required")
                                .WithData("QuestionId", qId);
                        break;

                    case QuestionType.SingleChoice:
                        if (dto.SelectedOptionIds == null || dto.SelectedOptionIds.Count != 1)
                            throw new BusinessException("Submit.Validation.SingleChoice.OneRequired")
                                .WithData("QuestionId", qId);
                        break;

                    case QuestionType.MultiChoice:
                        if (dto.SelectedOptionIds == null || dto.SelectedOptionIds.Count == 0)
                            throw new BusinessException("Submit.Validation.MultiChoice.AtLeastOneRequired")
                                .WithData("QuestionId", qId);
                        break;
                }
            }

            instance.MarkSubmitted();
            await _surveyInstanceRepository.UpdateAsync(instance, autoSave: true);
        }

        [Authorize(SurveyManagerPermissions.Responses.Submit)]
        [UnitOfWork] // ensures both save + submit happen in a single transaction
        public virtual async Task SaveAndSubmitResponsesBulkAsync(SaveResponsesBulkDto input)
        {
            // 1) Save (draft/partial). This will also mark instance InProgress if required.
            await SaveResponsesBulkAsync(input);

            // 2) After saving, attempt to submit. This will perform submit-time validations.
            await SubmitSurveyInstanceAsync(input.SurveyInstanceId);
        }
        private async Task EnsureAssigneeAndEditableAsync(SurveyInstance instance)
        {
            // only assignee can save response
            if(instance.AssigneeUserId != CurrentUser.GetId())
            {
                throw new BusinessException("SurveyInstance.Save.NotAssignee")
                    .WithData("AssigneeUserId", instance.AssigneeUserId)
                    .WithData("CurrentUserId", CurrentUser.GetId());
            }

            // must be in an editable state
            if(instance.Status==SurveyInstanceStatus.Expired || instance.Status == SurveyInstanceStatus.Submitted)
            {
                throw new BusinessException("SurveyInstance.Save.NotEditableState")
                    .WithData("Status", instance.Status.ToString());
            }

            // must be before duedate
            if(instance.DueDate <= DateTime.UtcNow)
            {
                throw new BusinessException("SurveyInstance.Save.DueDatePassed")
            .WithData("DueDate", instance.DueDate.Value);
            }
        }

        private async Task ValidateSaveResponseDtoAsync(SaveResponseDto dto)
        {
            var question = await _questionRepository.GetAsync(dto.QuestionId);

            //validate based on question type 
            switch (question.Type)
            {
                case QuestionType.Likert1To5:
                case QuestionType.Likert1To7:
                    await ValidateLikertAsync(dto, question.Type);
                    break;

                case QuestionType.SingleChoice:
                    await ValidateSingleChoiceAsync(dto);
                    break;

                case QuestionType.MultiChoice:
                    await ValidateMultiChoiceAsync(dto);
                    break;

                default:
                    throw new BusinessException("Response.Validation.UnknownQuestionType")
                        .WithData("QuestionId", dto.QuestionId)
                        .WithData("Type", question.Type.ToString());
            }
        }

        private Task ValidateLikertAsync(SaveResponseDto dto, QuestionType type)
        {
            // Draft mode (Assigned/InProgress) allows null
            if (dto.AnswerValue == null)
                return Task.CompletedTask;

            int value = dto.AnswerValue.Value;

            if (type == QuestionType.Likert1To5 && (value < 1 || value > 5))
            {
                throw new BusinessException("Response.Validation.LikertOutOfRange")
                    .WithData("ExpectedRange", "1–5")
                    .WithData("Actual", value);
            }

            if (type == QuestionType.Likert1To7 && (value < 1 || value > 7))
            {
                throw new BusinessException("Response.Validation.LikertOutOfRange")
                    .WithData("ExpectedRange", "1–7")
                    .WithData("Actual", value);
            }

            // Choice lists must be empty
            if (dto.SelectedOptionIds != null && dto.SelectedOptionIds.Any())
            {
                throw new BusinessException("Response.Validation.LikertMustNotHaveOptions");
            }

            return Task.CompletedTask;
        }

        private async Task ValidateSingleChoiceAsync(SaveResponseDto dto)
        {
            if (dto.SelectedOptionIds == null || !dto.SelectedOptionIds.Any())
                return; // draft saves can leave it empty

            if (dto.SelectedOptionIds.Count != 1)
            {
                throw new BusinessException("Response.Validation.SingleChoice.MustHaveOneOption");
            }

            await ValidateOptionsBelongToQuestionAsync(dto);
        }

        private async Task ValidateMultiChoiceAsync(SaveResponseDto dto)
        {
            if (dto.SelectedOptionIds == null || !dto.SelectedOptionIds.Any())
                return; // draft allows empty

            await ValidateOptionsBelongToQuestionAsync(dto);
        }

        private async Task ValidateOptionsBelongToQuestionAsync(SaveResponseDto dto)
        {
            var optionIds = dto.SelectedOptionIds ?? new List<Guid>();

            if (!optionIds.Any())
                return;

            var options = await _optionRepository.GetListAsync(o =>
                optionIds.Contains(o.Id) && o.QuestionId == dto.QuestionId);

            if (options.Count != optionIds.Count)
            {
                throw new BusinessException("Response.Validation.OptionMismatch")
                    .WithData("QuestionId", dto.QuestionId)
                    .WithData("ProvidedOptionCount", optionIds.Count)
                    .WithData("ValidOptionCount", options.Count);
            }
        }

        [Authorize(SurveyManagerPermissions.Indicators.ViewAll)]
        public async Task<List<QuestionResponseSummaryDto>> GetResponsesByQuestionIdAsync(Guid questionId)
        {
            // Get all responses for this question
            var responses = await _responseRepository.GetListAsync(r => r.QuestionId == questionId);

            if (!responses.Any())
            {
                return new List<QuestionResponseSummaryDto>();
            }

            // Get survey instance IDs to check their status
            var surveyInstanceIds = responses.Select(r => r.SurveyInstanceId).Distinct().ToList();
            var surveyInstances = await _surveyInstanceRepository.GetListAsync(si => surveyInstanceIds.Contains(si.Id));
            var instanceStatusMap = surveyInstances.ToDictionary(si => si.Id, si => si.Status);

            // Get response options for multi-choice questions
            var responseIds = responses.Select(r => r.Id).ToList();
            var responseOptions = await _responseOptionRepository.GetListAsync(ro => responseIds.Contains(ro.ResponseId));
            var responseOptionsMap = responseOptions.GroupBy(ro => ro.ResponseId)
                .ToDictionary(g => g.Key, g => g.Select(ro => ro.OptionId).ToList());

            // Build summary DTOs
            var summaries = responses.Select(r => new QuestionResponseSummaryDto
            {
                ResponseId = r.Id,
                SurveyInstanceId = r.SurveyInstanceId,
                AnswerValue = r.AnswerValue,
                SelectedOptionIds = responseOptionsMap.ContainsKey(r.Id) ? responseOptionsMap[r.Id] : new List<Guid>(),
                IsSubmitted = instanceStatusMap.ContainsKey(r.SurveyInstanceId) && 
                              instanceStatusMap[r.SurveyInstanceId] == SurveyInstanceStatus.Submitted,
                CreationTime = r.CreationTime
            }).ToList();

            return summaries;
        }
    }
}
