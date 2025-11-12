using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.SurveyManager.Surveys.Enums
{
    public enum OptionDataType
    {
        String = 0,   // default - plain text like "Yes", "Red", "Satisfied"
        Int = 1,      // whole numbers like "1", "10"
        Decimal = 2,  // decimal numbers like "3.5", "99.99"
        Bool = 3,     // boolean values like "true", "false", "1", "0"
        Date = 4,     // date only, like "2025-11-12"
        DateTime = 5  // full timestamp like "2025-11-12T15:30:00Z"
    }
}
