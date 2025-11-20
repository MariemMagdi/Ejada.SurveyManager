using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ejada.SurveyManager.SurveyInstances
{
    public class Response : FullAuditedEntity<Guid>
    {
        public Guid SurveyInstanceId { get; private set; }
        public Guid QuestionId { get; private set; }
        public int? AnswerValue { get; private set; }

        private Response() { }

        private Response(Guid id, Guid surveyInstanceId, Guid questionId, int? answerValue)
            : base(id)
        {
            SetSurveyInstance(surveyInstanceId);
            SetQuestion(questionId);
            SetAnswerValue(answerValue);
        }

        public static Response Create(Guid id, Guid surveyInstanceId, Guid questionId, int? answerValue = null)
        {
            return new Response(id, surveyInstanceId, questionId, answerValue);
        } 

        public Response SetSurveyInstance(Guid surveyInstanceId) 
        {
            if(surveyInstanceId == Guid.Empty)
            {
                throw new BusinessException("Response.SurveyInstanceId.Invalid")
                    .WithData("SurveyInstanceId", surveyInstanceId);
            }
            SurveyInstanceId = surveyInstanceId;
            return this;
        }

        public Response SetQuestion(Guid questionId) 
        {
            if (questionId == Guid.Empty)
            {
                throw new BusinessException("Response.QuestionId.Invalid")
                    .WithData("QuestionInstanceId", questionId);
            }
            QuestionId = questionId;
            return this;
        }

        public Response SetAnswerValue(int? answerValue) 
        {
            AnswerValue = answerValue;
            return this;
        }
    }
}
