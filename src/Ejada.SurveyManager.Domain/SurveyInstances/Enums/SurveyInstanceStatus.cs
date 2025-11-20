using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejada.SurveyManager.SurveyInstances.Enums
{
    public enum SurveyInstanceStatus
    {
        Assigned = 0,   // created & assigned to employee; no answers yet
        InProgress = 1, // employee saved draft (at least one answer saved)
        Submitted = 2,  // employee clicked submit before due date (locked)
        Expired = 3     // due date passed without submit (locked)
        // Future Options:
        // Reopened = 4, // admin reopens after Submitted/Expired
        // Canceled = 5  // admin cancels this assignment
    }
}
