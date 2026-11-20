# School Safety Incident Reporting and Management System

**Student:** Gaju Diego  
**Student ID:** 27395  
**Technology:** C# · ASP.NET Core 10 MVC · Entity Framework Core · SQL Server

---

## Overview

A web-based platform for reporting, recording and managing safety incidents within schools. School members can report incidents such as bullying, theft, accidents and physical violence. Administrators and safety officers can review reports, manage incidents and monitor their progress.

---

## Features

| Feature | Description |
|---|---|
| User Authentication | Register, login and logout securely using ASP.NET Identity |
| Role-Based Access | Administrator, Safety Officer, Reporter roles with restricted access |
| Incident Reporting | Submit incidents with title, description, date, location and categories |
| Witness & Evidence | Record witness details and upload supporting evidence files |
| Status Tracking | Track incidents: Pending → Under Investigation → Resolved |
| Search & Filter | Filter by category, location, date range, status and keyword |
| Dashboard | Statistics overview with recent incidents and category breakdown |
| Location Management | Admin can add, edit and delete school locations |
| Category Management | Admin can add, edit and delete incident categories |
| User Management | Admin can assign roles and delete user accounts |

---

## User Roles

- **Administrator** — Full access: manages users, locations, categories and all incidents
- **Safety Officer** — Reviews, investigates and updates incident statuses
- **Reporter** — Submits incidents and monitors their own reports

---

## Tech Stack

- **Backend:** C# · ASP.NET Core 10 MVC
- **ORM:** Entity Framework Core 10
- **Database:** SQL Server (LocalDB)
- **Auth:** ASP.NET Core Identity
- **Frontend:** Bootstrap 5 · Bootstrap Icons

---

## Database Tables

| Table | Description |
|---|---|
| AspNetUsers | Stores user accounts and roles (Identity) |
| Incidents | Stores reported incidents |
| Locations | Stores school building and room information |
| IncidentDetails | Stores witness information and evidence file paths |
| IncidentCategories | Stores incident categories |
| IncidentCategoryMaps | Many-to-many link between incidents and categories |

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server or SQL Server LocalDB
- Git

### Setup

```bash
# Clone the repository
git clone https://github.com/diegogaju/SchoolSafetySystem.git
cd SchoolSafetySystem

# Apply database migrations
dotnet ef database update

# Run the application
dotnet run
```

Then open `https://localhost:5001` in your browser.

### Default Admin Account

| Field | Value |
|---|---|
| Email | admin@school.com |
| Password | Admin@123 |

---

## Project Structure

```
SchoolSafetySystem/
├── Controllers/
│   ├── AccountController.cs
│   ├── DashboardController.cs
│   ├── IncidentsController.cs
│   ├── CategoriesController.cs
│   ├── LocationsController.cs
│   └── UsersController.cs
├── Models/
│   ├── ApplicationUser.cs
│   ├── Incident.cs
│   ├── IncidentCategory.cs
│   ├── IncidentCategoryMap.cs
│   ├── IncidentDetail.cs
│   └── Location.cs
├── ViewModels/
│   ├── AccountViewModels.cs
│   └── IncidentViewModels.cs
├── Data/
│   ├── ApplicationDbContext.cs
│   └── DbSeeder.cs
├── Views/
│   ├── Account/
│   ├── Dashboard/
│   ├── Incidents/
│   ├── Categories/
│   ├── Locations/
│   └── Users/
├── Migrations/
├── wwwroot/
└── Program.cs
```

---

## Screenshots

> Dashboard, incident list, report form and status update screens available on first run.

---

## License

This project was developed for academic purposes — GAJU DIEGO · ID 27395.
