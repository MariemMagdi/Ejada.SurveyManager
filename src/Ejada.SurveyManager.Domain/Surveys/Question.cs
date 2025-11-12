using Ejada.SurveyManager.Common;
using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ejada.SurveyManager.Surveys
{
    public class Question : FullAuditedEntity<Guid>
    {
        public Guid SurveyId { get; private set; }
        public string Text { get; private set; }
        public QuestionType Type { get; private set; }

        // Navigation Child
        private readonly List<Option> _options = new();
        public IReadOnlyCollection<Option> Options => _options.AsReadOnly();

        private Question() { } // EF Core

        private Question(Guid id, Guid surveyId, string text, QuestionType type) : base(id)
        {
            SetSurvey(surveyId);
            SetText(text);
            SetType(type);
        }

        public static Question Create(Guid id, Guid surveyId, string text, QuestionType type) =>
            new Question(id, surveyId, text, type);

        private Question SetSurvey(Guid surveyId) 
        {
            if(surveyId == Guid.Empty)
            {
                throw new BusinessException("Question.SurveyId.Invalid");
            }
            SurveyId = surveyId;
            return this;
        }
        public Question SetText(string text) 
        {
            Check.NotNullOrWhiteSpace(text, nameof(text));
            if(text.Length > DomainConstants.QuestionTextMaxLength)
            {
                throw new BusinessException("Question.Text.MaxLength").WithData("Max", DomainConstants.QuestionTextMaxLength);
            }
            Text = text.Trim();
            return this;
        }
        public Question SetType(QuestionType newType) 
        {
            if (Type == newType) return this;
            if((newType==QuestionType.Likert1To5 || newType == QuestionType.Likert1To7) && _options.Count > 0)
            {
                throw new BusinessException("Question.ChangeType.IncompatibleWithOptions")
                    .WithData("QuestionId", Id)
                    .WithData("ExistingOptionsCount", _options.Count);
            }
            Type = newType;
            return this;
        }

        //Child Behavior
        public Option AddOption(Guid id, string label, OptionDataType type) 
        {
            if(Type == QuestionType.Likert1To5 || Type == QuestionType.Likert1To7)
            {
                throw new BusinessException("Question.Options.NotAllowedForLikert");
            }
            var option = Option.Create(id, this.Id, label, type);
            _options.Add(option);
            return option;
        }
        public bool RemoveOption(Guid optionId) 
        {
            var idx = _options.FindIndex(option => option.Id == optionId);
            if (idx >= 0)
            {
                _options.RemoveAt(idx);
                return true;
            }
            return false;
        }
        public void ClearOptions() 
        {
            _options.Clear();
        }
    }
}
