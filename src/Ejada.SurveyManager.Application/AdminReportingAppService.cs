using Ejada.SurveyManager.Reporting.Dtos;
using Ejada.SurveyManager.SurveyInstances;
using Ejada.SurveyManager.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Ejada.SurveyManager.Reporting
{
    public class AdminReportingAppService : ApplicationService, IAdminReportingAppService
    {
        private readonly IRepository<SurveyInstance, Guid> _surveyInstanceRepository;
        private readonly IRepository<Survey, Guid> _surveyRepository;
        private readonly IRepository<Response, Guid> _responseRepository;
        private readonly IRepository<ResponseOption, Guid> _responseOptionRepository;
        private readonly IRepository<Question, Guid> _questionRepository;

        public AdminReportingAppService(
            IRepository<SurveyInstance, Guid> surveyInstanceRepository,
            IRepository<Survey, Guid> surveyRepository,
            IRepository<Response, Guid> responseRepository,
            IRepository<ResponseOption, Guid> responseOptionRepository,
            IRepository<Question, Guid> questionRepository)
        {
            _surveyInstanceRepository = surveyInstanceRepository;
            _surveyRepository = surveyRepository;
            _responseRepository = responseRepository;
            _responseOptionRepository = responseOptionRepository;
            _questionRepository = questionRepository;
        }

        public async Task<PagedResultDto<SubmittedSurveyInstanceDto>> GetSubmittedSurveyInstancesAsync(GetSubmittedSurveyInstancesInput input)
        {
            // Base query: only submitted instances
            var query = (await _surveyInstanceRepository.GetQueryableAsync())
                .Where(si => si.Status == Ejada.SurveyManager.SurveyInstances.Enums.SurveyInstanceStatus.Submitted);

            if (input.SurveyId.HasValue)
                query = query.Where(si => si.SurveyId == input.SurveyId.Value);

            if (input.AssigneeUserId.HasValue)
                query = query.Where(si => si.AssigneeUserId == input.AssigneeUserId.Value);

            if (input.From.HasValue)
                query = query.Where(si => si.CreationTime >= input.From.Value);

            if (input.To.HasValue)
                query = query.Where(si => si.CreationTime <= input.To.Value);

            if (!string.IsNullOrWhiteSpace(input.Search))
            {
                var s = input.Search.Trim();
                // If surveyName is needed, we join surveys; to keep it simple, filter by SurveyId if parseable, else no-op
            }

            var total = await AsyncExecuter.CountAsync(query);

            // Apply sorting & paging
            query = query.OrderByDescending(si => si.CreationTime)
                         .Skip(input.SkipCount)
                         .Take(input.MaxResultCount);

            var items = await AsyncExecuter.ToListAsync(query);

            // Load survey names
            var surveyIds = items.Select(i => i.SurveyId).Distinct().ToList();
            var surveys = surveyIds.Any() ? await _surveyRepository.GetListAsync(s => surveyIds.Contains(s.Id)) : new List<Survey>();
            var surveysById = surveys.ToDictionary(s => s.Id, s => s);

            var dtos = items.Select(i => new SubmittedSurveyInstanceDto
            {
                Id = i.Id,
                SurveyId = i.SurveyId,
                SurveyName = surveysById.TryGetValue(i.SurveyId, out var sv) ? sv.Name : null,
                AssigneeUserId = i.AssigneeUserId,
                DueDate = i.DueDate,
                Status = i.Status,
                CreationTime = i.CreationTime
            }).ToList();

            return new PagedResultDto<SubmittedSurveyInstanceDto>(total, dtos);
        }

        public async Task<SubmissionDetailDto> GetSubmissionDetailAsync(Guid surveyInstanceId)
        {
            // Only allow fetching if instance is submitted
            var instance = await _surveyInstanceRepository.GetAsync(surveyInstanceId);
            if (instance.Status != Ejada.SurveyManager.SurveyInstances.Enums.SurveyInstanceStatus.Submitted)
            {
                throw new BusinessException("Reporting.View.NotSubmitted");
            }

            var survey = await _surveyRepository.GetAsync(instance.SurveyId);

            var responses = await _responseRepository.GetListAsync(r => r.SurveyInstanceId == surveyInstanceId);
            var responseIds = responses.Select(r => r.Id).ToList();
            var responseOptions = responseIds.Any()
                ? await _responseOptionRepository.GetListAsync(ro => responseIds.Contains(ro.ResponseId))
                : new List<ResponseOption>();
            var responseOptionsByResponse = responseOptions.GroupBy(ro => ro.ResponseId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.OptionId).ToList());

            var questionIds = responses.Select(r => r.QuestionId).Distinct().ToList();
            var questions = questionIds.Any() ? await _questionRepository.GetListAsync(q => questionIds.Contains(q.Id)) : new List<Question>();
            var questionsById = questions.ToDictionary(q => q.Id, q => q.Text);

            var responseDetails = responses.Select(r => new SubmittedResponseDetailDto
            {
                QuestionId = r.QuestionId,
                QuestionText = questionsById.TryGetValue(r.QuestionId, out var t) ? t : string.Empty,
                AnswerValue = r.AnswerValue,
                SelectedOptionIds = responseOptionsByResponse.TryGetValue(r.Id, out var so) ? so : new List<Guid>()
            }).ToList();

            var result = new SubmissionDetailDto
            {
                Id = instance.Id,
                SurveyInstanceId = instance.Id,
                SurveyId = instance.SurveyId,
                SurveyName = survey?.Name,
                AssigneeUserId = instance.AssigneeUserId,
                DueDate = instance.DueDate,
                Status = instance.Status,
                CreationTime = instance.CreationTime,
                Responses = responseDetails
            };

            return result;
        }
    }
}
