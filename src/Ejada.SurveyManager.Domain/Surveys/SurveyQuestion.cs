using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ejada.SurveyManager.Surveys
{
    public class SurveyQuestion : FullAuditedEntity<Guid>
    {
        public Guid SurveyId { get; private set; }
        public Guid QuestionId { get; private set; }

        private SurveyQuestion() { }

        public SurveyQuestion(Guid id ,Guid surveyId, Guid questionId)
            :base(id)
        {
            SurveyId = surveyId;
            QuestionId = questionId;
        }
    }
}
