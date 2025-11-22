using System;
using Volo.Abp.Domain.Entities;

namespace Ejada.SurveyManager.Indicators
{
    /// <summary>
    /// Many-to-many relationship between Questions and Indicators
    /// </summary>
    public class QuestionIndicator : Entity
    {
        public Guid QuestionId { get; set; }
        public Guid IndicatorId { get; set; }

        private QuestionIndicator() { } // EF Core

        public QuestionIndicator(Guid questionId, Guid indicatorId)
        {
            QuestionId = questionId;
            IndicatorId = indicatorId;
        }

        public override object[] GetKeys()
        {
            return new object[] { QuestionId, IndicatorId };
        }
    }
}

