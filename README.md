# Survey Management System

A comprehensive survey management platform built with ASP.NET Core and ABP Framework, enabling organizations to create, distribute, and analyze surveys with a modern Blazor-based user interface.

## Table of Contents

- [Project Overview](#project-overview)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [System Architecture](#system-architecture)
- [Database Design](#database-design)
- [Setup & Installation](#setup--installation)
- [Configuration](#configuration)
- [User Roles & Permissions](#user-roles--permissions)
- [Localization](#localization)
- [Development Notes](#development-notes)
- [Future Improvements](#future-improvements)
- [Screenshots](#screenshots)

## Project Overview

The Survey Management System is an enterprise-grade application designed to streamline the entire survey lifecycle from creation to analysis. It serves multiple user roles within an organization:

- **Administrators**: Full control over surveys, questions, indicators, and assignments
- **Managers**: Ability to assign surveys to employees and view aggregated responses
- **Employees**: Access to assigned surveys, ability to answer questions, and view their own responses

### High-Level Features

- **Survey Creation & Management**: Create surveys with custom questions, set target audiences, and manage survey lifecycle
- **Question Management**: Build reusable question libraries with multiple question types (Single Choice, Multi Choice, Likert Scale, Text)
- **Survey Assignment**: Assign surveys to specific employees with optional due dates
- **Survey Responses**: Employees can answer assigned surveys with draft saving and submission capabilities
- **Indicator Management**: Link questions to performance indicators for analytics
- **Dashboard**: Real-time overview of surveys, assignments, and statistics
- **Localization**: Full support for English and Arabic languages
- **Role-Based Access Control**: Granular permissions system integrated with ABP Framework

## Tech Stack

- **.NET 9.0**: Latest .NET framework version
- **ASP.NET Core**: Web application framework
- **ABP Framework 9.3.5**: Modular application framework providing:
  - Dependency injection
  - Authorization & authentication
  - Multi-tenancy support
  - Localization infrastructure
  - Audit logging
- **Blazor WebAssembly**: Client-side UI framework
- **Blazorise 1.8.1**: Component library with Bootstrap 5 styling
- **Entity Framework Core**: ORM for database operations
- **SQL Server**: Relational database management system
- **OpenIddict**: Authentication server for OAuth 2.0 / OpenID Connect

## Features

### Survey Management

- Create and edit surveys with metadata (name, purpose, target audience)
- Activate/deactivate surveys for assignment
- Attach existing questions or create new questions inline
- View survey details with all associated questions and options
- Delete surveys (with validation for linked assignments)

### Question Management

- Create questions with multiple types:
  - **Single Choice**: One option selection
  - **Multi Choice**: Multiple option selection
  - **Likert Scale**: Predefined 5-point or 7-point scale
- Define options with typed values (String, Int, Decimal, Bool, Date, DateTime)
- Link questions to indicators for analytics
- Reusable question library
- Protection against editing questions linked to active surveys
- Client-side validation for option data types

### Indicator Management

- Create performance indicators with descriptions
- Link multiple questions to indicators
- View response statistics per indicator
- Active/inactive indicator status management

### Survey Assignment

- Assign surveys to specific employees
- Set optional due dates with visual warnings
- Track assignment status:
  - **Assigned**: Not yet started
  - **In Progress**: Draft saved
  - **Submitted**: Completed
  - **Expired**: Past due date
- View all assignments or filter by assignee
- Mark assignments as expired manually

### Survey Responses & Statistics

- Employees can answer assigned surveys
- Save drafts for later completion
- Submit completed surveys
- View response details including:
  - Selected options for choice questions
  - Text responses
  - Likert scale ratings
- Response statistics per question:
  - Total responses
  - Option distribution
  - Average ratings (for Likert scales)
  - Percentage breakdowns

### Dashboard

- Overview statistics:
  - Total surveys count
  - Active surveys count
  - Total assignments count
  - Total indicators count
- Assignment status breakdown with visual indicators
- Recently created surveys list
- Latest survey assignments with assignee information
- Quick navigation to detailed views

### Authentication & Authorization

- OpenIddict-based authentication
- Role-based access control (RBAC)
- Permission-based authorization:
  - Survey management permissions (Create, Edit, Delete)
  - Question management permissions (Create, Edit, Delete)
  - Survey instance permissions (ViewOwn, ViewAll, Create, Edit, Delete, MarkExpired)
  - Response permissions (ViewOwn, ViewAll, Answer, Submit)
  - Indicator permissions (ViewAll, Create, Edit, Delete)
- Custom login page with modern UI

### Localization

- Full support for English (en) and Arabic (ar)
- JSON-based localization resources
- Localized UI text, validation messages, and error messages
- Culture-specific date/time formatting
- Right-to-left (RTL) support for Arabic

## System Architecture

The solution follows ABP Framework's layered architecture pattern, ensuring separation of concerns and maintainability:

### Domain Layer (`Ejada.SurveyManager.Domain`)

Contains the core business logic and entities:
- **Entities**: `Survey`, `Question`, `Option`, `SurveyInstance`, `Response`, `ResponseOption`, `Indicator`, `QuestionIndicator`
- **Domain Services**: Business logic encapsulated within entities
- **Value Objects**: Domain constants and enums
- **Repository Interfaces**: Defined here, implemented in EF Core layer

### Application Layer (`Ejada.SurveyManager.Application`)

Implements application services and business workflows:
- **Application Services**: `SurveyAppService`, `QuestionAppService`, `SurveyInstanceAppService`, `ResponseAppService`, `IndicatorAppService`
- **DTOs**: Data transfer objects for input/output
- **AutoMapper Profiles**: Entity to DTO mappings
- **Business Logic**: Orchestrates domain operations

### Application Contracts Layer (`Ejada.SurveyManager.Application.Contracts`)

Defines public contracts:
- **DTOs**: Shared data transfer objects
- **Interfaces**: Application service interfaces
- **Permissions**: Permission definitions
- **Enums**: Shared enumerations

### Domain Shared Layer (`Ejada.SurveyManager.Domain.Shared`)

Shared resources across layers:
- **Localization**: JSON files for en/ar languages
- **Constants**: Domain constants
- **Enums**: Shared enumerations

### HTTP API Layer (`Ejada.SurveyManager.HttpApi`)

RESTful API controllers:
- Exposes application services as HTTP endpoints
- Handles request/response serialization
- API versioning support

### HTTP API Host (`Ejada.SurveyManager.HttpApi.Host`)

API hosting and configuration:
- Startup configuration
- Middleware pipeline
- Authentication server (OpenIddict)
- Database context configuration
- Virtual file system for localization

### Blazor Client (`Ejada.SurveyManager.Blazor.Client`)

WebAssembly-based UI:
- **Pages**: Razor components for all features
- **Navigation**: Menu configuration
- **Components**: Reusable UI components
- **Services**: Client-side service implementations
- **Localization**: Client-side localization setup

### Entity Framework Core Layer (`Ejada.SurveyManager.EntityFrameworkCore`)

Data access implementation:
- **DbContext**: `SurveyManagerDbContext`
- **Entity Configurations**: Fluent API configurations for all entities
- **Migrations**: Database schema migrations
- **Repositories**: EF Core repository implementations

### Database Migrator (`Ejada.SurveyManager.DbMigrator`)

Standalone application for:
- Running database migrations
- Seeding initial data
- Development and production database setup

## Database Design

The system uses a relational database model with the following main entities:

### Core Entities

**Survey**
- Represents a survey with metadata (name, purpose, target audience, active status)
- Links to multiple questions through `SurveyQuestion` junction table
- Supports soft delete and audit fields

**Question**
- Contains question text and type (SingleChoice, MultiChoice, Likert, Text)
- Has multiple options (for choice types)
- Can be linked to multiple indicators
- Protected from editing when linked to active surveys

**Option**
- Belongs to a question
- Has a label and data type (String, Int, Decimal, Bool, Date, DateTime)
- Validated against its data type

**SurveyInstance**
- Represents a survey assignment to an employee
- Links a survey to an assignee user
- Tracks status and optional due date
- Supports status transitions

**Response**
- Stores an employee's answer to a question
- Links to a survey instance and question
- Contains answer value (text or reference to options)

**ResponseOption**
- Junction table for multi-choice responses
- Links responses to selected options

**Indicator**
- Performance indicator with name and description
- Links to multiple questions for analytics
- Active/inactive status

**QuestionIndicator**
- Junction table linking questions to indicators
- Enables many-to-many relationship

### Relationships

- Survey → SurveyQuestion → Question (Many-to-Many)
- Question → Option (One-to-Many)
- SurveyInstance → Survey (Many-to-One)
- SurveyInstance → User (Many-to-One)
- Response → SurveyInstance (Many-to-One)
- Response → Question (Many-to-One)
- Response → ResponseOption → Option (Many-to-Many)
- Indicator → QuestionIndicator → Question (Many-to-Many)

All entities include ABP audit fields (CreationTime, CreatorId, LastModificationTime, LastModifierId, IsDeleted, DeleterId, DeletionTime).

## Setup & Installation

### Prerequisites

- **.NET 9.0 SDK** or later
- **SQL Server 2019** or later (or SQL Server Express)
- **Visual Studio 2022** or **Visual Studio Code** (optional, for development)
- **Git** (for cloning the repository)

### Step 1: Clone the Repository

```bash
git clone <repository-url>
cd Ejada.SurveyManager
```

### Step 2: Restore NuGet Packages

```bash
dotnet restore
```

### Step 3: Configure Database Connection

Update the connection string in `src/Ejada.SurveyManager.HttpApi.Host/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=SurveyManagerDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

For SQL Server authentication, use:
```
Server=localhost;Database=SurveyManagerDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True
```

### Step 4: Apply Database Migrations

Run the database migrator:

```bash
cd src/Ejada.SurveyManager.DbMigrator
dotnet run
```

This will:
- Create the database if it doesn't exist
- Apply all EF Core migrations
- Seed initial data (admin user, permissions, etc.)

### Step 5: Run the Application

#### Option A: Run API Host and Blazor Client Separately

**Terminal 1 - API Host:**
```bash
cd src/Ejada.SurveyManager.HttpApi.Host
dotnet run
```

**Terminal 2 - Blazor Client:**
```bash
cd src/Ejada.SurveyManager.Blazor.Client
dotnet run
```

#### Option B: Run from Visual Studio

1. Set `Ejada.SurveyManager.HttpApi.Host` as startup project
2. Set `Ejada.SurveyManager.Blazor.Client` as startup project (for Blazor)
3. Run both projects

### Step 6: Access the Application

- **Blazor Client**: `https://localhost:44300` (or the port shown in console)
- **API Host**: `https://localhost:44301` (or the port shown in console)
- **Swagger UI**: `https://localhost:44301/swagger`

### Default Credentials

After running the database migrator, default admin credentials are created:
- **Username**: `admin`
- **Password**: `1q2w3E*` (check `appsettings.json` or seed data for actual password)

## Configuration

### Localization Configuration

Localization files are located in:
- `src/Ejada.SurveyManager.Domain.Shared/Localization/SurveyManager/en.json`
- `src/Ejada.SurveyManager.Domain.Shared/Localization/SurveyManager/ar.json`

The localization is configured in:
- `src/Ejada.SurveyManager.Domain.Shared/SurveyManagerDomainSharedModule.cs`
- `src/Ejada.SurveyManager.HttpApi.Host/SurveyManagerHttpApiHostModule.cs`
- `src/Ejada.SurveyManager.Blazor.Client/SurveyManagerBlazorClientModule.cs`

### Database Connection

Connection strings are configured in:
- `src/Ejada.SurveyManager.HttpApi.Host/appsettings.json`
- `src/Ejada.SurveyManager.DbMigrator/appsettings.json`

### Environment Variables

For production deployments, use environment variables or `appsettings.Production.json`:
- `ConnectionStrings__Default`: Database connection string
- `App__SelfUrl`: Application self URL for OpenIddict
- `AuthServer__Authority`: Authentication server authority

## User Roles & Permissions

### Admin Capabilities

Administrators have full access to all features:
- Create, edit, and delete surveys
- Manage question library
- Create and manage indicators
- Assign surveys to any employee
- View all survey assignments and responses
- Mark assignments as expired
- Manage user permissions

### Employee Capabilities

Employees have limited access:
- View their own assigned surveys (`SurveyInstances.ViewOwn`)
- Answer assigned surveys (`Responses.Answer`)
- Submit survey responses (`Responses.Submit`)
- View their own responses (`Responses.ViewOwn`)
- Save drafts of incomplete surveys

### Permission System

The system uses ABP Framework's permission system with the following permission groups:

**SurveyManager.Surveys**
- `Create`: Create new surveys
- `Edit`: Edit existing surveys
- `Delete`: Delete surveys

**SurveyManager.Questions**
- `Create`: Create new questions
- `Edit`: Edit existing questions
- `Delete`: Delete questions

**SurveyManager.SurveyInstances**
- `ViewOwn`: View own assigned surveys
- `ViewAll`: View all survey assignments
- `Create`: Assign surveys to employees
- `Edit`: Edit survey assignments
- `Delete`: Delete survey assignments
- `MarkExpired`: Mark assignments as expired

**SurveyManager.Responses**
- `ViewOwn`: View own responses
- `ViewAll`: View all responses
- `Answer`: Answer assigned surveys
- `Submit`: Submit survey responses

**SurveyManager.Indicators**
- `ViewAll`: View all indicators
- `Create`: Create indicators
- `Edit`: Edit indicators
- `Delete`: Delete indicators

Permissions are defined in `src/Ejada.SurveyManager.Application.Contracts/Permissions/SurveyManagerPermissions.cs` and registered in `SurveyManagerPermissionDefinitionProvider.cs`.

## Localization

### How It Works

The system uses ABP Framework's localization infrastructure with JSON-based resources. Localization keys are stored in JSON files and loaded at runtime.

### File Structure

Localization files are located at:
```
src/Ejada.SurveyManager.Domain.Shared/Localization/SurveyManager/
├── en.json (English)
└── ar.json (Arabic)
```

### Adding New Keys

1. Open the appropriate JSON file (`en.json` or `ar.json`)
2. Add a new key-value pair in the `Texts` object:
   ```json
   {
     "Culture": "en",
     "Texts": {
       "YourNewKey": "Your New Value"
     }
   }
   ```
3. Use the key in Razor pages: `@L["YourNewKey"]`
4. Add the same key to both `en.json` and `ar.json` with appropriate translations

### Using Localization in Code

**In Razor Pages:**
```razor
@L["Survey Name"]
```

**In C# Code:**
```csharp
@inject IStringLocalizer<SurveyManagerResource> L
var text = L["Survey Name"];
```

**For Validation Messages:**
```csharp
@inject AbpBlazorMessageLocalizerHelper<SurveyManagerResource> LH
<Validation MessageLocalizer="@LH.Localize">
```

### Localization Resource

The localization resource is defined in:
- `src/Ejada.SurveyManager.Domain.Shared/Localization/SurveyManagerResource.cs`
- Registered in `SurveyManagerDomainSharedModule.cs`

## Development Notes

### Adding a New Entity/Module

1. **Create Domain Entity** (`Domain` layer):
   - Add entity class inheriting from `FullAuditedEntity<Guid>`
   - Add domain methods and business logic
   - Define repository interface

2. **Create EF Core Configuration** (`EntityFrameworkCore` layer):
   - Add `IEntityTypeConfiguration<T>` implementation
   - Configure table name, columns, relationships
   - Register in `SurveyManagerDbContext.OnModelCreating`

3. **Create DTOs** (`Application.Contracts` layer):
   - Create DTOs for input/output
   - Add AutoMapper mappings

4. **Create Application Service** (`Application` layer):
   - Implement `CrudAppService` or custom service
   - Add business logic
   - Register in module

5. **Create API Controller** (`HttpApi` layer):
   - Add controller inheriting from ABP base controllers
   - Expose application service methods

6. **Create Blazor Pages** (`Blazor.Client` layer):
   - Add Razor pages for list, create, edit, details
   - Use `AbpCrudPageBase` for CRUD operations
   - Add navigation menu items

7. **Add Permissions** (`Application.Contracts` layer):
   - Add permission constants
   - Register in `PermissionDefinitionProvider`

8. **Add Localization Keys** (`Domain.Shared` layer):
   - Add keys to `en.json` and `ar.json`

9. **Create Migration**:
   ```bash
   cd src/Ejada.SurveyManager.EntityFrameworkCore
   dotnet ef migrations add AddNewEntity --startup-project ../Ejada.SurveyManager.HttpApi.Host
   ```

### Wiring Pages with Localization

1. Add `@using Ejada.SurveyManager.Localization` to Razor page
2. Use `@L["Key"]` for all user-facing text
3. Add keys to both `en.json` and `ar.json`
4. For validation messages, use `AbpBlazorMessageLocalizerHelper<SurveyManagerResource>`

### Extending Dashboard

The dashboard is located at `src/Ejada.SurveyManager.Blazor.Client/Pages/Dashboard.razor`.

To add new statistics:
1. Inject required application services
2. Add data fetching logic in `OnInitializedAsync`
3. Add UI cards with statistics
4. Update localization keys if needed

### Extending Indicators

Indicators are managed in:
- Domain: `src/Ejada.SurveyManager.Domain/Indicators/`
- Application: `src/Ejada.SurveyManager.Application/Indicators/`
- UI: `src/Ejada.SurveyManager.Blazor.Client/Pages/Indicators/`

To add new indicator features:
1. Extend `Indicator` entity if needed
2. Update DTOs and application service
3. Modify UI pages
4. Add localization keys

## Future Improvements

The following enhancements are planned for future releases:

### Advanced Analytics
- Real-time dashboard with charts and graphs
- Trend analysis over time
- Comparative analysis between surveys
- Custom report generation

### Export Functionality
- Export survey responses to Excel
- Generate PDF reports
- Export survey templates
- Bulk data export

### Question Templates Marketplace
- Pre-built question templates library
- Community-contributed templates
- Template categories and tags
- Template rating and reviews

### Email Notifications
- Email notifications for new survey assignments
- Reminder emails for pending surveys
- Completion confirmation emails
- Weekly/monthly summary emails

### Additional Features
- Survey scheduling and automation
- Conditional question logic
- File upload questions
- Survey branching based on answers
- Anonymous survey support
- Survey sharing via public links
- Advanced filtering and search
- Bulk survey operations
- API rate limiting and throttling
- Audit trail for all operations

## Screenshots

_Screenshots will be added here to showcase the application's user interface and key features._

### Dashboard
_Add screenshot of the main dashboard showing statistics and overview_

### Survey Management
_Add screenshot of the surveys list page_

### Question Creation
_Add screenshot of the question creation form_

### Survey Assignment
_Add screenshot of assigning a survey to an employee_

### Survey Response
_Add screenshot of an employee answering a survey_

### Response Statistics
_Add screenshot of response analytics and statistics_

---

## License

[Specify your license here]

## Contributing

[Add contribution guidelines if applicable]

## Support

For issues, questions, or contributions, please [specify your support channel].
