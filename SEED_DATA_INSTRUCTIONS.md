# Seed Data Instructions

## How to Run the Test Data Seeder

The `SurveyManagerTestDataSeedContributor` will automatically seed realistic test data when you run the DbMigrator.

### Steps to Seed Data:

1. **Build the solution:**
   ```bash
   dotnet build
   ```

2. **Run the DbMigrator:**
   ```bash
   dotnet run --project src/Ejada.SurveyManager.DbMigrator
   ```

3. **Wait for completion:**
   - The migrator will apply any pending migrations
   - Then it will seed the test data
   - You'll see log messages like:
     - "Seeding test data..."
     - "Seeding users..."
     - "Seeding indicators..."
     - "Seeding questions..."
     - "Seeding surveys..."
     - "Seeding survey instances..."
     - "Seeding responses..."
     - "Test data seeding completed successfully."

4. **Idempotent seeding:**
   - Running the migrator multiple times is safe
   - The seeder checks if data already exists (by checking for Survey1Id)
   - If test data exists, it will skip seeding and log: "Test data already seeded. Skipping..."

### What Gets Seeded:

#### **Users (4 total):**
- **admin** (default ABP admin) - already exists
- **john.smith@test.com** - Employee role
- **sarah.johnson@test.com** - Employee role
- **mike.wilson@test.com** - Auditor role
- Password for all: `1q2w3E*`

#### **Indicators (4 total):**
- Employee Satisfaction
- Work Environment
- Training Effectiveness
- Communication Quality

#### **Questions (10 total):**
- 4 Likert questions (2x 1-5 scale, 2x 1-7 scale)
- 3 Single Choice questions (with 4-5 options each)
- 3 Multi Choice questions (with 4-5 options each)
- Some linked to indicators, some not

#### **Surveys (8 total):**
- 7 Active surveys with varying numbers of questions
- 1 Inactive survey
- 1 Empty survey (no questions) for testing edge cases

#### **Survey Assignments (7 total):**
- John Smith: 4 assignments
  - 1 Submitted (fully answered)
  - 1 In Progress (partially answered, due soon)
  - 1 Assigned (not started, future due date)
  - 1 Assigned (empty survey)
- Sarah Johnson: 3 assignments
  - 1 Expired (overdue, not answered)
  - 1 Submitted (fully answered)
  - 1 Assigned (not started, no due date)

#### **Responses:**
- Fully answered surveys have all questions completed
- Partially answered surveys have some questions answered
- Various answer types: Likert ratings, single choice, multi choice

### Resetting Test Data:

If you want to reset and reseed the data:

1. **Option A: Delete specific test data via UI**
   - Login as admin
   - Delete survey assignments
   - Delete surveys (soft delete)
   - Delete questions (if not linked)
   - Delete indicators
   - Re-run DbMigrator

2. **Option B: Drop and recreate database**
   ```bash
   # Drop the database (SQL Server example)
   sqlcmd -S localhost -Q "DROP DATABASE SurveyManager"
   
   # Run migrator to recreate and seed
   dotnet run --project src/Ejada.SurveyManager.DbMigrator
   ```

3. **Option C: Delete test users and re-run**
   - Delete the test users from Identity tables
   - Re-run DbMigrator
   - Test data will be recreated

### Verifying Seed Data:

After seeding, you can verify by:

1. **Login as admin:**
   - Navigate to Surveys → should see 8 surveys
   - Navigate to Questions → should see 10 questions
   - Navigate to Indicators → should see 4 indicators
   - Navigate to Survey Assignments → should see 7 assignments

2. **Login as john.smith:**
   - Navigate to My Surveys → should see 4 assignments
   - Can answer surveys, save drafts, submit

3. **Login as mike.wilson (Auditor):**
   - Can view all surveys, questions, indicators, assignments
   - Cannot create/edit/delete (read-only)

4. **Login as sarah.johnson:**
   - Navigate to My Surveys → should see 3 assignments
   - One expired survey (overdue)

### Troubleshooting:

**Issue:** "Test data already seeded. Skipping..."
- **Solution:** This is normal if data already exists. To reseed, delete the test data first.

**Issue:** User creation fails
- **Solution:** Ensure the Employee and Auditor roles exist in your system. They should be created by the standard ABP seed data.

**Issue:** Foreign key constraint errors
- **Solution:** Ensure all migrations are applied before seeding. Run: `dotnet ef database update --project src/Ejada.SurveyManager.EntityFrameworkCore`

**Issue:** Duplicate key errors
- **Solution:** Test data uses fixed GUIDs. If data already exists, the seeder will skip. Delete existing test data first.

### Test Data IDs (for reference):

All test data uses fixed GUIDs for consistency:

- **Surveys:** `10000000-0000-0000-0000-00000000000X` (X = 1-8)
- **Questions:** `20000000-0000-0000-0000-00000000000X` (X = 1-10)
- **Indicators:** `30000000-0000-0000-0000-00000000000X` (X = 1-4)
- **Survey Instances:** `40000000-0000-0000-0000-00000000000X` (X = 1-7)
- **Users:**
  - Employee 1: `3a15cfcd-4b32-0e12-a1cb-4b2e957db826`
  - Employee 2: `4b25cfcd-5c43-1f23-b2dc-5c3f068ec937`
  - Auditor: `5c35cfcd-6d54-2034-c3ed-6d40179fd048`

These fixed IDs make it easy to reference specific test data in automated tests or manual testing scenarios.

### Next Steps:

After seeding, proceed to the **MANUAL_TEST_SCENARIOS.md** file for comprehensive testing instructions.

