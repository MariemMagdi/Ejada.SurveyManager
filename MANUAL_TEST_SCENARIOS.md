# Survey Manager - Manual Test Scenarios

## Test Users

| Username | Email | Password | Role | User ID |
|----------|-------|----------|------|---------|
| admin | admin@abp.io | 1q2w3E* | Admin | (default ABP admin) |
| john.smith | john.smith@test.com | 1q2w3E* | Employee | 3a15cfcd-4b32-0e12-a1cb-4b2e957db826 |
| sarah.johnson | sarah.johnson@test.com | 1q2w3E* | Employee | 4b25cfcd-5c43-1f23-b2dc-5c3f068ec937 |
| mike.wilson | mike.wilson@test.com | 1q2w3E* | Auditor | 5c35cfcd-6d54-2034-c3ed-6d40179fd048 |

## Test Data Overview

### Surveys (8 total)
1. **Annual Employee Satisfaction Survey 2024** - Active, 4 questions
2. **Q1 2024 Feedback Survey** - Active, 2 questions
3. **Training Program Evaluation** - Active, 2 questions
4. **Department Information Survey** - Active, 2 questions
5. **Benefits and Workplace Survey** - Active, 6 questions
6. **Future Survey Template** - Active, **NO QUESTIONS** (for testing empty survey)
7. **Archived 2023 Survey** - **INACTIVE**, 2 questions
8. **Exit Interview Survey** - Active, 4 questions

### Survey Assignments (7 total)
1. **John Smith** - Annual Survey - **Submitted** - Fully answered
2. **John Smith** - Q1 Feedback - **In Progress** - Partially answered - **Due in 12 hours**
3. **John Smith** - Training Evaluation - **Assigned** - Not started - Due in 5 days
4. **Sarah Johnson** - Annual Survey - **Expired** - Not answered - **Overdue by 3 days**
5. **Sarah Johnson** - Department Survey - **Submitted** - Fully answered
6. **Sarah Johnson** - Benefits Survey - **Assigned** - Not started - **No due date**
7. **John Smith** - Future Survey Template - **Assigned** - **Empty survey (no questions)**

### Indicators (4 total)
- Employee Satisfaction (linked to 4 questions)
- Work Environment (linked to 3 questions)
- Training Effectiveness (linked to 1 question)
- Communication Quality (linked to 1 question)

---

## Feature 1: Survey Management (Admin)

### Scenario 1.1: View All Surveys
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys" from the main menu
2. Observe the surveys list

**Expected Results:**
- 8 surveys displayed in the data grid
- Columns show: Survey Name, Purpose, Target Audience, Is Active (badge), Actions
- "Annual Employee Satisfaction Survey 2024" shows green "Active" badge
- "Archived 2023 Survey" shows gray "Inactive" badge
- Each row has Edit, View Details, and Delete actions

---

### Scenario 1.2: Create New Survey
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys"
2. Click "New Survey" button
3. Fill in:
   - Survey Name: "Test New Survey"
   - Purpose: "Testing survey creation"
   - Target Audience: "Test Users"
   - Check "Is Active"
4. Add 2 inline questions (Likert 1-5)
5. Attach 1 existing question from predefined list
6. Click "Save"

**Expected Results:**
- Survey created successfully
- Redirected to surveys list
- New survey appears in the list
- All 3 questions are linked to the survey

---

### Scenario 1.3: Edit Survey with Questions
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys"
2. Click "Edit" on "Q1 2024 Feedback Survey"
3. Change Purpose to "Updated quarterly feedback"
4. Remove one existing question
5. Add one new predefined question
6. Click "Save"

**Expected Results:**
- Survey updated successfully
- Changes reflected in survey details
- Question links updated correctly

---

### Scenario 1.4: View Survey Details
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys"
2. Click "View Details" on "Annual Employee Satisfaction Survey 2024"

**Expected Results:**
- Survey details page displays
- Shows: Name, Purpose, Target Audience, Active status
- Lists all 4 linked questions with their text and type
- Shows options for choice-type questions
- Back button returns to surveys list

---

### Scenario 1.5: View Empty Survey Details
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys"
2. Click "View Details" on "Future Survey Template"

**Expected Results:**
- Survey details page displays
- Shows survey information
- Message displayed: "No questions in this survey"
- No questions list shown

---

### Scenario 1.6: Delete Survey
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys"
2. Click "Delete" on "Archived 2023 Survey"
3. Confirm deletion in the popup

**Expected Results:**
- Confirmation message: "Are you sure you want to delete the survey 'Archived 2023 Survey'?"
- After confirmation, survey is soft-deleted
- Survey no longer appears in the list
- Questions that were only linked to this survey become editable again

---

## Feature 2: Question Management (Admin)

### Scenario 2.1: View All Questions
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions" from the main menu
2. Observe the questions list

**Expected Results:**
- 10 questions displayed
- Columns show: Question Text, Question Type, Options (count), Status, Actions
- Questions linked to active surveys show yellow "Linked to survey" badge
- Unlinked questions show green "Editable" badge
- Linked questions have no Edit/Delete actions
- Unlinked questions have Edit, View Details, and Delete actions

---

### Scenario 2.2: Create New Question (Likert)
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Click "New Question" button
3. Fill in:
   - Question Text: "How satisfied are you with the new system?"
   - Question Type: "Likert 1 to 5"
4. Select linked indicators (optional)
5. Click "Save"

**Expected Results:**
- Question created successfully
- Redirected to questions list
- New question appears with type "Likert1To5"
- Shows 0 options (Likert questions don't have options)
- Status shows "Editable" (not linked to any survey)

---

### Scenario 2.3: Create New Question (Single Choice)
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Click "New Question"
3. Fill in:
   - Question Text: "What is your preferred work location?"
   - Question Type: "Single Choice"
4. Add 3 options:
   - "Office"
   - "Remote"
   - "Hybrid"
5. Link to "Work Environment" indicator
6. Click "Save"

**Expected Results:**
- Question created successfully
- Question appears with 3 options
- Indicator link established
- Status shows "Editable"

---

### Scenario 2.4: Edit Unlinked Question
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Find a question with "Editable" status
3. Click "Edit"
4. Modify question text
5. Add/remove options (if applicable)
6. Change linked indicators
7. Click "Save"

**Expected Results:**
- Question updated successfully
- Changes reflected in questions list
- Indicator links updated

---

### Scenario 2.5: Attempt to Edit Linked Question
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Find "How satisfied are you with your current role?" (linked to Survey 1)
3. Observe available actions

**Expected Results:**
- Question shows yellow "Linked to survey" badge
- Edit button is NOT visible
- Delete button is NOT visible
- Only "View Details" action is available
- Warning message explains question cannot be edited because it's linked to active surveys

---

### Scenario 2.6: View Question Details with Indicators
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Click "View Details" on "How satisfied are you with your current role?"

**Expected Results:**
- Question details page displays
- Shows question text and type
- Shows options (if applicable)
- "Linked Indicators" section displays:
  - "Employee Satisfaction" indicator with green "Active" badge
- Back button returns to questions list

---

### Scenario 2.7: View Question Details without Indicators
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Click "View Details" on "Which department do you work in?"

**Expected Results:**
- Question details page displays
- Shows question text, type, and 4 options (IT, HR, Finance, Operations)
- "Linked Indicators" section shows: "No indicators are linked to this question."

---

### Scenario 2.8: Delete Unlinked Question
**Preconditions:** Logged in as `admin`

**Steps:**
1. Create a new test question (not linked to any survey)
2. Navigate to "Questions"
3. Click "Delete" on the test question
4. Confirm deletion

**Expected Results:**
- Confirmation message appears
- Question and its options are deleted
- Question removed from list

---

### Scenario 2.9: Attempt to Delete Linked Question
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Find a question with "Linked to survey" badge
3. Observe available actions

**Expected Results:**
- Delete button is NOT visible
- Question cannot be deleted while linked to active surveys

---

## Feature 3: Indicator Management (Admin & Auditor)

### Scenario 3.1: View All Indicators (Admin)
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Indicators" from the main menu
2. Observe the indicators list

**Expected Results:**
- 4 indicators displayed
- Columns show: Indicator Name, Description, Is Active, Linked Questions (count), Actions
- "Employee Satisfaction" shows "4 Questions" badge
- All indicators show green "Active" badge
- Each row has View Details, Edit, and Delete actions

---

### Scenario 3.2: View All Indicators (Auditor)
**Preconditions:** Logged in as `mike.wilson` (Auditor)

**Steps:**
1. Navigate to "Indicators" from the main menu
2. Observe the indicators list

**Expected Results:**
- 4 indicators displayed (same as admin)
- All data is visible
- Only "View Details" action is available
- Edit and Delete buttons are NOT visible (read-only access)

---

### Scenario 3.3: Create New Indicator
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Indicators"
2. Click "Create Indicator" button
3. Fill in:
   - Indicator Name: "Leadership Quality"
   - Description: "Measures leadership effectiveness"
   - Check "Is Active"
4. Select 2-3 questions to link
5. Click "Save"

**Expected Results:**
- Indicator created successfully
- Modal closes
- New indicator appears in the list
- Shows correct count of linked questions
- Questions are now linked to this indicator

---

### Scenario 3.4: Edit Indicator
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Indicators"
2. Click "Edit" on "Work Environment"
3. Change description
4. Add or remove linked questions
5. Click "Save"

**Expected Results:**
- Indicator updated successfully
- Modal closes
- Changes reflected in indicators list
- Question links updated

---

### Scenario 3.5: View Indicator Details with Responses
**Preconditions:** Logged in as `admin` or `mike.wilson` (Auditor)

**Steps:**
1. Navigate to "Indicators"
2. Click "View Details" on "Employee Satisfaction"

**Expected Results:**
- Indicator details page displays
- Shows indicator name, description, and active status
- "Linked Questions" section shows all 4 linked questions
- For each question:
  - Question text and type displayed
  - Options listed (if applicable)
  - **Response Statistics** section shows:
    - Total Responses count
    - Submitted Responses count
    - For Likert questions: Average Rating (e.g., "4.25 / 5")
    - For choice questions: Option Distribution table with counts and percentages
- Questions with no responses show "No responses have been recorded yet."

---

### Scenario 3.6: View Indicator Details without Questions
**Preconditions:** Logged in as `admin`

**Steps:**
1. Create a new indicator without linking any questions
2. Navigate to "Indicators"
3. Click "View Details" on the new indicator

**Expected Results:**
- Indicator details page displays
- Shows indicator information
- Message displayed: "No questions are linked to this indicator."
- No questions or statistics shown

---

### Scenario 3.7: Delete Indicator
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Indicators"
2. Click "Delete" on "Communication Quality"
3. Confirm deletion

**Expected Results:**
- Confirmation message: "Are you sure you want to delete the indicator 'Communication Quality'?"
- After confirmation, indicator is deleted
- Indicator removed from list
- Questions previously linked to this indicator are unaffected (just the link is removed)

---

### Scenario 3.8: Auditor Cannot Edit/Delete
**Preconditions:** Logged in as `mike.wilson` (Auditor)

**Steps:**
1. Navigate to "Indicators"
2. Observe available actions

**Expected Results:**
- "Create Indicator" button is NOT visible
- Only "View Details" action available for each indicator
- Edit and Delete buttons are NOT visible
- Auditor has read-only access

---

## Feature 4: Survey Assignments (Admin)

### Scenario 4.1: View All Survey Assignments
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments" from the main menu
2. Observe the assignments list

**Expected Results:**
- 7 survey assignments displayed
- Columns show: Survey, Assignee User (email), Due Date, Status (badge), Actions
- Statuses displayed with colored badges:
  - "Assigned" - blue badge
  - "In Progress" - yellow badge
  - "Submitted" - green badge
  - "Expired" - red badge
- Due dates formatted as "yyyy-MM-dd HH:mm"
- Assignments with no due date show "No Due Date"
- John Smith's Q1 Feedback shows due date with "(Due Soon)" indicator
- Sarah Johnson's Annual Survey shows "(Overdue)" indicator

---

### Scenario 4.2: Create Survey Assignment
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Click "Assign Survey" button
3. Select Survey: "Exit Interview Survey"
4. Select Employee: "john.smith@test.com"
5. Set Due Date: 7 days from now (optional)
6. Click "Assign"

**Expected Results:**
- Assignment created successfully
- Modal closes
- New assignment appears in the list
- Status is "Assigned"
- Employee receives the assignment (visible in "My Surveys")

---

### Scenario 4.3: Create Assignment without Due Date
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Click "Assign Survey"
3. Select Survey and Employee
4. Leave Due Date empty
5. Click "Assign"

**Expected Results:**
- Assignment created successfully
- Due Date column shows "No Due Date"
- Assignment has no expiration

---

### Scenario 4.4: View Submitted Survey Responses
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Find John Smith's "Annual Employee Satisfaction Survey 2024" (Status: Submitted)
3. Click "View Responses"

**Expected Results:**
- Survey Instance Details page displays
- Shows survey name, status (green "Submitted" badge), assignee, and due date
- "Responses" section shows all 4 questions with answers:
  - "How satisfied are you with your current role?" - Answer: 4 / 5
  - "Rate your work-life balance" - Answer: 5 / 7
  - "How would you rate your direct manager's support?" - Answer: 5 / 5
  - "Is your workload manageable?" - Answer: 3 / 5
- For choice questions, selected options are highlighted
- Back button returns to assignments list

---

### Scenario 4.5: View In-Progress Survey (No Responses Yet)
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Find an assignment with Status "Assigned" (not started)
3. Observe available actions

**Expected Results:**
- "View Responses" button is NOT visible
- Only "Mark as Expired" and "Delete" actions available
- Admin cannot view responses until survey is submitted

---

### Scenario 4.6: View Partially Answered Survey
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Find John Smith's "Q1 2024 Feedback Survey" (Status: In Progress)
3. Note: "View Responses" is NOT available (not submitted yet)

**Expected Results:**
- Status shows yellow "In Progress" badge
- "View Responses" button is NOT visible (survey not submitted)
- "Mark as Expired" and "Delete" actions available
- Admin must wait for submission to view responses

---

### Scenario 4.7: Mark Survey as Expired
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Find John Smith's "Training Program Evaluation" (Status: Assigned)
3. Click "Mark as Expired"
4. Confirm action

**Expected Results:**
- Confirmation message: "Are you sure you want to mark this survey instance as expired?"
- After confirmation, status changes to red "Expired" badge
- Employee can no longer answer the survey
- "Mark as Expired" action no longer available for this assignment

---

### Scenario 4.8: Delete Survey Assignment
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Click "Delete" on any assignment
3. Confirm deletion

**Expected Results:**
- Confirmation message appears
- Assignment is deleted
- Removed from list
- Employee no longer sees it in "My Surveys"

---

### Scenario 4.9: View Empty Survey Assignment
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Find John Smith's "Future Survey Template" assignment
3. Click "View Responses" (if submitted) or check status

**Expected Results:**
- Assignment exists but survey has no questions
- Employee cannot answer (nothing to answer)
- If viewing details, message shows: "No questions in this survey"

---

## Feature 5: My Surveys (Employee)

### Scenario 5.1: View My Assigned Surveys (John Smith)
**Preconditions:** Logged in as `john.smith` (Employee)

**Steps:**
1. Navigate to "My Surveys" from the main menu
2. Observe the surveys list

**Expected Results:**
- 4 survey assignments displayed (only John's assignments)
- Columns show: Survey, Due Date, Status, Actions
- Statuses displayed:
  - Annual Survey: green "Submitted" badge
  - Q1 Feedback: yellow "Draft" badge (In Progress)
  - Training Evaluation: blue "Not Started" badge (Assigned)
  - Future Survey Template: blue "Not Started" badge
- Q1 Feedback shows due date with yellow "(Due Soon)" warning
- Training Evaluation shows future due date
- Future Survey Template shows future due date
- Annual Survey shows "View Responses" action (submitted)
- Other surveys show "Answer Survey" action

---

### Scenario 5.2: View My Assigned Surveys (Sarah Johnson)
**Preconditions:** Logged in as `sarah.johnson` (Employee)

**Steps:**
1. Navigate to "My Surveys"
2. Observe the surveys list

**Expected Results:**
- 3 survey assignments displayed
- Statuses:
  - Annual Survey: red "Expired" badge (overdue)
  - Department Survey: green "Submitted" badge
  - Benefits Survey: blue "Not Started" badge
- Annual Survey shows red "(Overdue)" indicator
- Benefits Survey shows "No Due Date"
- Department Survey shows "View Responses" action
- Expired survey shows "Answer Survey" but will be read-only when opened

---

### Scenario 5.3: Answer New Survey (Start)
**Preconditions:** Logged in as `john.smith`

**Steps:**
1. Navigate to "My Surveys"
2. Find "Training Program Evaluation" (Status: Not Started)
3. Click "Answer Survey"

**Expected Results:**
- Survey answering page displays
- Shows survey name and due date
- Lists all questions with appropriate input controls:
  - Likert questions: Dropdown with rating scale
  - Single choice: Radio buttons
  - Multi choice: Checkboxes
- All fields are empty (not answered yet)
- "Cancel", "Save Draft", and "Submit" buttons available
- Due date reminder shown if applicable

---

### Scenario 5.4: Save Draft (Partial Answers)
**Preconditions:** Logged in as `john.smith`, on "Training Program Evaluation" survey

**Steps:**
1. Answer first question only
2. Leave second question unanswered
3. Click "Save Draft"

**Expected Results:**
- Draft saved successfully
- Returned to "My Surveys" list
- Survey status changes to yellow "Draft" badge (In Progress)
- Can return later to continue answering

---

### Scenario 5.5: Continue Answering Draft Survey
**Preconditions:** Logged in as `john.smith`

**Steps:**
1. Navigate to "My Surveys"
2. Find "Q1 2024 Feedback Survey" (Status: Draft, partially answered)
3. Click "Answer Survey"

**Expected Results:**
- Survey answering page displays
- Previously answered question shows saved answer (Question 1: rating 4)
- Unanswered question is still empty (Question 7)
- Can modify previous answers
- Can complete remaining questions
- "Save Draft" and "Submit" buttons available

---

### Scenario 5.6: Submit Completed Survey
**Preconditions:** Logged in as `john.smith`, on a survey with all questions answered

**Steps:**
1. Ensure all questions are answered
2. Click "Submit"
3. Confirm submission (if prompted)

**Expected Results:**
- Survey submitted successfully
- Returned to "My Surveys" list
- Survey status changes to green "Submitted" badge
- Action changes from "Answer Survey" to "View Responses"
- Survey becomes read-only (cannot edit answers)
- Admin/Auditor can now view responses

---

### Scenario 5.7: View My Submitted Responses
**Preconditions:** Logged in as `john.smith`

**Steps:**
1. Navigate to "My Surveys"
2. Find "Annual Employee Satisfaction Survey 2024" (Status: Submitted)
3. Click "View Responses"

**Expected Results:**
- Survey details page displays in read-only mode
- Shows all questions with submitted answers
- Message at top: "This survey has been submitted or expired and cannot be edited"
- All input controls are disabled
- No "Save" or "Submit" buttons
- Only "Back" button available

---

### Scenario 5.8: Attempt to Answer Expired Survey
**Preconditions:** Logged in as `sarah.johnson`

**Steps:**
1. Navigate to "My Surveys"
2. Find "Annual Employee Satisfaction Survey 2024" (Status: Expired, red badge)
3. Click "Answer Survey"

**Expected Results:**
- Survey page opens in read-only mode
- Message displayed: "This survey has been submitted or expired and cannot be edited"
- All input controls are disabled
- Cannot save or submit answers
- Survey shows as overdue with red indicator

---

### Scenario 5.9: Answer Empty Survey
**Preconditions:** Logged in as `john.smith`

**Steps:**
1. Navigate to "My Surveys"
2. Find "Future Survey Template" (empty survey with no questions)
3. Click "Answer Survey"

**Expected Results:**
- Survey page displays
- Message shown: "No questions in this survey"
- No questions to answer
- Only "Back" button available
- Cannot save or submit (nothing to submit)

---

### Scenario 5.10: Survey with No Due Date
**Preconditions:** Logged in as `sarah.johnson`

**Steps:**
1. Navigate to "My Surveys"
2. Find "Benefits and Workplace Survey" (No Due Date)
3. Observe due date column

**Expected Results:**
- Due Date column shows "No Due Date" in gray text
- No overdue or due soon indicators
- Survey can be answered anytime
- No time pressure

---

## Feature 6: Auditor Access

### Scenario 6.1: Auditor Views Survey Assignments
**Preconditions:** Logged in as `mike.wilson` (Auditor)

**Steps:**
1. Navigate to "Survey Assignments"
2. Observe the assignments list

**Expected Results:**
- All 7 survey assignments visible (same as admin)
- Can see all employees' assignments
- "Assign Survey" button is NOT visible
- Can only "View Responses" for submitted surveys
- Cannot Mark as Expired or Delete assignments
- Read-only access to view data

---

### Scenario 6.2: Auditor Views Submitted Responses
**Preconditions:** Logged in as `mike.wilson` (Auditor)

**Steps:**
1. Navigate to "Survey Assignments"
2. Find a submitted survey (e.g., John Smith's Annual Survey)
3. Click "View Responses"

**Expected Results:**
- Can view full survey responses
- Same view as admin
- Shows all questions and answers
- Read-only access (cannot modify)

---

### Scenario 6.3: Auditor Views Indicators with Statistics
**Preconditions:** Logged in as `mike.wilson` (Auditor)

**Steps:**
1. Navigate to "Indicators"
2. Click "View Details" on any indicator

**Expected Results:**
- Can view indicator details
- Can see all linked questions
- Can see response statistics and analytics
- Same data as admin
- Cannot edit or delete indicators

---

### Scenario 6.4: Auditor Cannot Access Employee Features
**Preconditions:** Logged in as `mike.wilson` (Auditor)

**Steps:**
1. Check main menu

**Expected Results:**
- "My Surveys" menu item is NOT visible
- Auditor cannot answer surveys
- Auditor role is for viewing/auditing only

---

## Feature 7: Permission & Access Control

### Scenario 7.1: Employee Cannot Access Admin Features
**Preconditions:** Logged in as `john.smith` (Employee)

**Steps:**
1. Check main menu

**Expected Results:**
- "Surveys" menu item is NOT visible
- "Questions" menu item is NOT visible
- "Indicators" menu item is NOT visible
- "Survey Assignments" menu item is NOT visible
- Only "My Surveys" is visible
- Employee has limited access to their own surveys only

---

### Scenario 7.2: Employee Cannot Access Other Employees' Surveys
**Preconditions:** Logged in as `john.smith`

**Steps:**
1. Navigate to "My Surveys"
2. Observe the list

**Expected Results:**
- Only shows John's 4 assignments
- Cannot see Sarah's assignments
- Cannot see other employees' responses
- Access restricted to own surveys only

---

### Scenario 7.3: Admin Has Full Access
**Preconditions:** Logged in as `admin`

**Steps:**
1. Check main menu
2. Navigate through all features

**Expected Results:**
- All menu items visible: Surveys, Questions, Indicators, Survey Assignments, My Surveys
- Can create, edit, delete all entities
- Can view all employees' assignments and responses
- Full system access

---

## Feature 8: Data Validation & Edge Cases

### Scenario 8.1: Cannot Delete Survey with Active Assignments
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys"
2. Attempt to delete "Annual Employee Satisfaction Survey 2024" (has active assignments)
3. Confirm deletion

**Expected Results:**
- Survey is soft-deleted (IsDeleted = true)
- Survey no longer appears in surveys list
- Existing assignments remain intact
- Employees can still complete their assigned surveys
- Questions become unlinked and editable again

---

### Scenario 8.2: Cannot Edit Question Linked to Active Survey
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Find "How satisfied are you with your current role?" (linked to Survey 1)

**Expected Results:**
- Question shows "Linked to survey" badge
- Edit button is NOT visible
- Delete button is NOT visible
- Warning message explains restriction
- Question is protected from modification

---

### Scenario 8.3: Cannot Submit Survey with Unanswered Required Questions
**Preconditions:** Logged in as `john.smith`, on a survey

**Steps:**
1. Answer only some questions
2. Leave others blank
3. Click "Submit"

**Expected Results:**
- Validation error (if validation is implemented)
- OR survey submits with partial answers (current behavior)
- Can save as draft instead

---

### Scenario 8.4: View Survey with Mixed Question Types
**Preconditions:** Logged in as `john.smith`

**Steps:**
1. Navigate to "My Surveys"
2. Answer "Benefits and Workplace Survey" (has 6 questions of different types)

**Expected Results:**
- Displays correctly:
  - Likert 1-5: Dropdown (1-5)
  - Likert 1-7: Dropdown (1-7)
  - Single Choice: Radio buttons
  - Multi Choice: Checkboxes
- All controls work properly
- Can save and submit successfully

---

## Feature 9: Reporting & Analytics

### Scenario 9.1: View Response Statistics in Indicator Details
**Preconditions:** Logged in as `admin` or `mike.wilson`

**Steps:**
1. Navigate to "Indicators"
2. Click "View Details" on "Employee Satisfaction"

**Expected Results:**
- Shows 4 linked questions
- For each question with responses:
  - Total Responses: Shows count
  - Submitted Responses: Shows count (only submitted, not drafts)
  - For Likert questions: Average Rating calculated (e.g., "4.25 / 5")
  - For choice questions: Option Distribution table
- Questions without responses show "No responses have been recorded yet."

---

### Scenario 9.2: Verify Average Rating Calculation
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Indicators"
2. Click "View Details" on "Employee Satisfaction"
3. Find "How satisfied are you with your current role?" question
4. Verify average rating

**Expected Results:**
- Shows "Average Rating: 4.00 / 5"
- Calculation based on submitted responses only (John's answer: 4)
- Draft and expired surveys not included in average

---

### Scenario 9.3: Verify Option Distribution
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Indicators"
2. Click "View Details" on indicator linked to choice questions
3. View option distribution table

**Expected Results:**
- Table shows:
  - Option name
  - Count (number of times selected)
  - Percentage (% of submitted responses)
- Only submitted responses counted
- Percentages add up correctly

---

## Feature 10: Search & Filtering

### Scenario 10.1: Filter Surveys by Name
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Surveys"
2. Use the filter row
3. Type "Annual" in the Name column filter

**Expected Results:**
- Only "Annual Employee Satisfaction Survey 2024" displayed
- Other surveys hidden
- Filter works case-insensitive

---

### Scenario 10.2: Filter Questions by Type
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Questions"
2. Use the filter row
3. Filter by Question Type

**Expected Results:**
- Can filter to show only Likert, Single Choice, or Multi Choice questions
- Grid updates to show matching questions only

---

### Scenario 10.3: Filter Survey Assignments by Status
**Preconditions:** Logged in as `admin`

**Steps:**
1. Navigate to "Survey Assignments"
2. Use the filter row
3. Filter by Status column

**Expected Results:**
- Can filter to show only Assigned, In Progress, Submitted, or Expired
- Grid updates to show matching assignments only
- Useful for finding overdue or pending assignments

---

## Test Data Cleanup

### Scenario 11.1: Reset Test Data
**Preconditions:** Logged in as `admin`

**Steps:**
1. Delete all test survey assignments
2. Delete test surveys (soft delete)
3. Delete test questions (if not linked)
4. Delete test indicators

**Expected Results:**
- Test data cleaned up
- Can re-run DbMigrator to reseed test data
- System returns to clean state

---

## Notes for Testers

1. **Password for all test users:** `1q2w3E*`
2. **Idempotent seeding:** Running DbMigrator multiple times won't duplicate data
3. **Fixed GUIDs:** Test data uses fixed GUIDs for consistency
4. **Soft deletes:** Surveys are soft-deleted (IsDeleted flag), not physically removed
5. **Status transitions:** Survey instances can transition: Assigned → In Progress → Submitted or Expired
6. **Response visibility:** Admins/Auditors can only view responses after submission
7. **Due date indicators:** 
   - Overdue: Due date in the past
   - Due Soon: Due date within 24 hours
   - No Due Date: No time limit
8. **Empty surveys:** "Future Survey Template" has no questions (tests edge case)
9. **Inactive surveys:** "Archived 2023 Survey" is inactive (tests filtering)
10. **Indicator analytics:** Response statistics only count submitted surveys

---

## Expected Test Coverage

✅ **CRUD Operations:** All entities (Surveys, Questions, Indicators, Assignments)  
✅ **Role-Based Access:** Admin, Employee, Auditor permissions  
✅ **Survey Lifecycle:** Assigned → In Progress → Submitted/Expired  
✅ **Question Types:** Likert 1-5, Likert 1-7, Single Choice, Multi Choice  
✅ **Edge Cases:** Empty surveys, no questions, no indicators, no due date  
✅ **Data Relationships:** Survey-Question links, Question-Indicator links  
✅ **Response Management:** Draft, submit, view responses  
✅ **Analytics:** Average ratings, option distributions  
✅ **Validation:** Cannot edit linked questions, cannot delete linked surveys  
✅ **UI States:** Active/Inactive, Editable/Linked, Overdue/Due Soon  

---

**Total Scenarios:** 50+ comprehensive test cases covering all features and edge cases.

