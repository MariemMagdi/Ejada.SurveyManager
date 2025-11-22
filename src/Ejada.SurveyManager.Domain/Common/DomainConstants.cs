using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.SurveyManager.Common
{
    public static class DomainConstants
    {
        public const int SurveyNameMaxLength = 128;
        public const int SurveyPurposeMaxLength = 1024;
        public const int SurveyTargetAudienceMaxLength = 256;
        public const int QuestionTextMaxLength = 1024;
        public const int OptionLabelMaxLength = 256;

        public const int IndicatorNameMaxLength = 200;
        public const int IndicatorDescriptionMaxLength = 1000;
    }
}
