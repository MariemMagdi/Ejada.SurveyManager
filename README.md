## Survey Management System

A survey lifecycle platform built with ASP.NET Core and ABP Framework, enabling organizations to create, distribute, and analyze surveys through a modern Blazor WebAssembly UI.

## Project Overview

- **Purpose**: Manage end-to-end surveys (design, assignment, response collection, analytics, and reporting).
- **Main roles**:
  - **Admin** – full control over configuration, surveys, questions, indicators, assignments, and users.
  - **Auditor** – read-only / analytical access to indicators, responses, and reports.
  - **Employee** – completes assigned surveys and can review own responses.

## Tech Stack

- **Backend**: .NET 9, ASP.NET Core, ABP Framework 9.3.5, Entity Framework Core, SQL Server, OpenIddict.
- **Frontend**: Blazor WebAssembly, Blazorise (Bootstrap 5), custom modern styling.
- **Infrastructure & Cross-cutting**: ABP modules (DI, auth, multi-tenancy-ready, localization, audit logging).
- **Reporting**: **QuestPDF** for indicator-level and per-employee PDF performance reports.

## Core Capabilities

- **Survey & Question Management**
  - Create and manage surveys with metadata, lifecycle (activate/deactivate), and question composition.
  - Reusable question library with Single Choice, Multi Choice, Likert scales, and text questions.
  - Typed options, validation, and protection for questions used in active surveys.

- **Assignments, Responses & Analytics**
  - Assign surveys to employees with due dates and status tracking (Assigned, In Progress, Submitted, Expired).
  - Employees can save drafts and submit responses; per-question statistics and option distributions are available.
  - Indicator management links questions to performance indicators and surfaces per-indicator response statistics.

- **Dashboards & Reporting**
  - Dashboard with key counts (surveys, assignments, indicators) and recent activity.
  - **Indicator PDF report**: all linked questions, per-question stats, and detailed employee responses.
  - **Employee PDF report**: per-employee performance across indicators and questions.

- **Authentication, Authorization & Localization**
  - OpenIddict-based authentication with ABP role & permission system.
  - Typical permissions: manage surveys/questions/indicators, assign surveys, view own/all responses, export reports.
  - Built-in localization with **English / Arabic**, including RTL support for Arabic.

## System Architecture

- **Domain (`Ejada.SurveyManager.Domain`)**: Core entities and business rules (surveys, questions, indicators, responses, survey instances).
- **Application (`Ejada.SurveyManager.Application`)**: Application services orchestrating domain logic and reporting (CRUD, assignments, PDF exports).
- **Application Contracts (`Ejada.SurveyManager.Application.Contracts`)**: DTOs, service interfaces, permissions, shared enums.
- **Domain Shared (`Ejada.SurveyManager.Domain.Shared`)**: Shared localization resources and domain constants.
- **HTTP API (`Ejada.SurveyManager.HttpApi` + `HttpApi.Host`)**: REST endpoints, hosting, authentication, middleware, configuration.
- **Blazor Client (`Ejada.SurveyManager.Blazor.Client`)**: WebAssembly UI for surveys, questions, indicators, assignments, users, and dashboards.
- **Entity Framework Core (`Ejada.SurveyManager.EntityFrameworkCore`)**: DbContext, mappings, repositories, migrations.
- **DbMigrator (`Ejada.SurveyManager.DbMigrator`)**: Console app to apply migrations and seed initial data.

## Data Model Overview

- Relational SQL Server schema with ABP audited entities.
- Core concepts: **surveys**, **questions**, **options**, **indicators**, **survey instances (assignments)**, **responses**, and link tables for many‑to‑many relationships.
- Responses support both direct numeric/text values (e.g., Likert) and option selections, enabling rich statistics and PDF reporting.

## Setup & Installation

- **Prerequisites**
  - .NET 9 SDK, SQL Server (2019+ or Express), Git, and optionally Visual Studio 2022 / VS Code.

- **Quick setup**
  - **Clone & restore**
    - `git clone <repository-url>`
    - `cd Ejada.SurveyManager`
    - `dotnet restore`
  - **Configure DB connection**
    - Edit `src/Ejada.SurveyManager.HttpApi.Host/appsettings.json` → `ConnectionStrings:Default` (Windows or SQL auth).
  - **Run migrations & seed**
    - `cd src/Ejada.SurveyManager.DbMigrator`
    - `dotnet run`
  - **Run the apps**
    - API Host: `cd src/Ejada.SurveyManager.HttpApi.Host && dotnet run`
    - Blazor Client: `cd src/Ejada.SurveyManager.Blazor.Client && dotnet run`
  - **Access**
    - Blazor Client (SPA): `https://localhost:44300` (or console port).
    - HTTP API / Swagger: `https://localhost:44301/swagger`.
  - **Default admin (seeded)**
    - Username: `admin`
    - Password: `1q2w3E*` (or as configured in `appsettings.json` / seed).

## Configuration & Localization

- **Configuration files**
  - App settings: `src/Ejada.SurveyManager.HttpApi.Host/appsettings*.json`.
  - DbMigrator: `src/Ejada.SurveyManager.DbMigrator/appsettings*.json`.
  - Common production env vars:
    - `ConnectionStrings__Default` – database connection string.
    - `App__SelfUrl` – backend base URL for OpenIddict.
    - `AuthServer__Authority` – authentication server authority.

- **Localization**
  - Resources: `src/Ejada.SurveyManager.Domain.Shared/Localization/SurveyManager/en.json` and `ar.json`.
  - Localization configured via Domain Shared, HttpApi.Host, and Blazor Client modules.
  - All user-facing strings should use localization keys to support **en/ar** and RTL for Arabic.

## User Roles & Permissions

- **Roles**
  - **Admin**: full management of surveys, questions, indicators, survey instances, users, and permissions.
  - **Auditor**: read-only access to indicators, survey instances, responses, and PDF reports (indicator and employee).
  - **Employee**: view and complete own survey assignments; view own responses and drafts.

- **Example capabilities**
  - Admin: create/edit/delete surveys and questions, manage indicators, assign surveys, expire assignments, manage users/roles.
  - Auditor: view all indicators and linked questions, inspect survey instances and responses, export indicator and employee reports.
  - Employee: view own assignments, answer and submit surveys, save drafts, view own response history.

## Developer Notes

- Follow **ABP layered architecture** when adding features: Domain → EF Core mapping → Application (service + DTOs) → HttpApi → Blazor Client.
- Reuse existing patterns: `CrudAppService`, ABP permissions, localization keys, and the existing QuestPDF report services for new reports.
- When adding UI pages, register routes/navigation and add localized strings in both `en.json` and `ar.json`.

## Future Improvements

- Richer analytics dashboards (charts, trends, comparisons between surveys/indicators).
- Additional export formats (Excel/CSV, more PDF templates, bulk exports).
- Advanced survey logic (conditional branching, scheduling, anonymous/public links, file upload questions).
- Enhanced notifications (assignments, reminders, completion summaries, periodic digests).
- More powerful filtering and search across surveys, assignments, responses, and indicators.
- Extended auditing and rate limiting for security and compliance scenarios.

## Screenshots

The following screenshots are recommended to be placed under  
`src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/` and referenced as below.

### Dashboard

![Dashboard](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/dashboard.png)

### Survey Management

![Survey Management](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/surveyManagement.png)

### Question Creation

![Question Creation](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/questioncreation.png)

### Survey Assignment

![Survey Assignment](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/surveyassignment.png)

### Survey Response

![Survey Response](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/surveyresponse.png)

### Indicator Details

![Indicator Details](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/indicatordetails.png)

### User Management

![User Management](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/usermanagement.png)

### Indicator PDF Report

![Indicator PDF Report](src/Ejada.SurveyManager.Blazor.Client/wwwroot/images/screenshots/indicatorreport.png)

