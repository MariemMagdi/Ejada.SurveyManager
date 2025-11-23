using Ejada.SurveyManager.Common;
using Ejada.SurveyManager.Surveys.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Ejada.SurveyManager.Surveys
{
    public class Option : FullAuditedEntity<Guid>
    {
        public Guid QuestionId { get; private set; }
        public string Label { get; private set; }
        public OptionDataType Type { get; private set; }

        private Option() { } // EF Core

        private Option(Guid id, Guid questionId, string label, OptionDataType type) : base(id)
        {
            SetQuestion(questionId);
            SetType(type);
            SetLabel(label);
        }

        public static Option Create(Guid id, Guid questionId, string label, OptionDataType type) =>
            new Option(id, questionId, label, type);

        private Option SetQuestion(Guid questionId)
        {
            if(questionId== Guid.Empty)
            {
                throw new BusinessException("Option.QuestionId.Invalid").WithData("QuestionId", questionId);
            }
            QuestionId = questionId;
            return this;
        }

        public Option SetType(OptionDataType newType)
        {
            if (newType == Type) return this;
            // Allow type change if label is null or empty (during initial creation)
            if (!string.IsNullOrWhiteSpace(Label) && !IsLabelParsableToType(Label, newType))
            {
                throw new BusinessException("Option.ChangeType.IncompatibleWithLabel")
                    .WithData("OptionId", Id)
                    .WithData("ExistingLabel", Label)
                    .WithData("RequestedType", newType.ToString());
            }
            Type = newType;
            return this;
        }

        public Option UpdateTypeAndLabel(OptionDataType newType, string newLabel)
        {
            Check.NotNullOrWhiteSpace(newLabel, nameof(newLabel));

            if (newLabel.Length > DomainConstants.OptionLabelMaxLength)
                throw new BusinessException("Option.Label.MaxLength")
                    .WithData("Max", DomainConstants.OptionLabelMaxLength);

            if (!IsLabelParsableToType(newLabel, newType))
            {
                throw new BusinessException("Option.ChangeType.LabelMismatch")
                    .WithData("RequestedType", newType.ToString())
                    .WithData("ProvidedLabel", newLabel);
            }

            Type = newType;
            Label = newLabel.Trim();
            return this;
        }

        private Option SetLabel(string label)
        {
            Check.NotNullOrWhiteSpace(label, nameof(label));
            if(label.Length > DomainConstants.OptionLabelMaxLength)
            {
                throw new BusinessException("Option.Label.MaxLength").WithData("Max", DomainConstants.OptionLabelMaxLength);
            }

            // Validate that label can be parsed to the chosen type
            if(!IsLabelParsableToType(label,Type))
            {
                throw new BusinessException("Option.Label.TypeMismatch").WithData("Label", label).WithData("ExpectedType", Type.ToString()).WithData("Type", Type);
            }
            Label = label.Trim();
            return this;
        }

        public bool TryGetTypedValue(out object typedValue)
        {
            try
            {
                typedValue = ParseLabelToType(Label, Type);
                return true;
            }
            catch
            {
                typedValue = null!;
                return false;
            }
        }


        public object GetTypedValue()
        {
            return ParseLabelToType(Label, Type);
        }


        private static bool IsLabelParsableToType(string label, OptionDataType type)
        {
            try
            {
                _ = ParseLabelToType(label, type);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static object ParseLabelToType(string label, OptionDataType type)
        {
            if (label is null) throw new ArgumentNullException(nameof(label));

            switch(type)
            {
                case OptionDataType.String:
                    return label.Trim();

                case OptionDataType.Int:
                    {
                        if(int.TryParse(label.Trim(),NumberStyles.Integer,CultureInfo.InvariantCulture, out var intVal))
                        {
                            return intVal;
                        }
                        throw new FormatException("Label is not a Valid Int.");
                    }

                case OptionDataType.Decimal:
                    {
                        if (decimal.TryParse(label.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var decVal))
                        {
                            return decVal;
                        }
                        throw new FormatException("Label is not a Valid Decimal.");
                    }

                case OptionDataType.Bool:
                    {
                        var s = label.Trim();
                        if (bool.TryParse(s, out var b)) return b;
                        if (s == "0") return false;
                        if (s == "1") return true;
                        throw new FormatException("Label is not a Valid Bool.");
                    }

                case OptionDataType.Date:
                    {
                        var s = label.Trim();
                        // Try ISO yyyy-MM-dd first
                        if (DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var d1))
                            return d1.Date;
                        // Fallback to general parse (invariant)
                        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var d2))
                            return d2.Date;
                        throw new FormatException("Label is not a valid Date.");
                    }

                case OptionDataType.DateTime:
                    {
                        var s = label.Trim();
                        // Try ISO round-trip first
                        if (DateTime.TryParseExact(s, "o", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dtO))
                            return dtO;
                        // Fallback to general invariant parse
                        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dt2))
                            return dt2;
                        throw new FormatException("Label is not a valid DateTime.");
                    }
                default:
                    return label.Trim();
            }
        }


    }
}
