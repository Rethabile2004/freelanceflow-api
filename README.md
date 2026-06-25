# FreelanceFlow API

A RESTful API for freelance business management — built for freelancers who need to track clients, projects, tasks, invoices, and payments in one place.

> 🚧 **Status: In active development.** This README will be updated as features are completed.

## Why this project exists

Most beginner backend portfolios are full of `Student/Course/Enrollment` or `Book/Author/Category` CRUD apps. FreelanceFlow is different — it models a real business problem: a freelancer managing clients, running projects, billing for work, and tracking partial payments against invoices. The goal is to demonstrate backend skills in a context that looks like something a company would actually pay a developer to build.

## Domain Overview

```
User (freelancer)
 └──< Client
         ├──< Project
         │       ├──< Task
         │       └──< Invoice
         │               └──< Payment
         └──< Note
```

- A **User** (the freelancer) manages many **Clients**
- A **Client** can have many **Projects** and **Notes**
- A **Project** can have many **Tasks** and **Invoices**
- An **Invoice** can have many **Payments** (clients often pay in installments)

### A note on Invoice status

Invoice status (`Pending`, `PartiallyPaid`, `Paid`, `Overdue`) is **calculated dynamically** from the sum of its Payments compared to its Amount — it is never stored as a field on the Invoice itself. This avoids a class of bugs where a stored status field falls out of sync with the actual payment data (e.g. a payment is added but the status field isn't updated, leaving the database contradicting itself).

## Tech Stack

- **ASP.NET Core Web API** (.NET 8)
- **Entity Framework Core** with **PostgreSQL**
- **ASP.NET Core Identity** + **JWT Bearer authentication** (with refresh tokens)
- **Repository Pattern** (applied deliberately here as a learning objective)
- **Docker** for containerization
- **Swagger / OpenAPI** for interactive API documentation
- Deployed on **Render**

## Planned Features

### Authentication
- [ ] Register / Login / Refresh token

### Client Management
- [ ] Full CRUD
- [ ] Notes per client

### Project Management
- [ ] Full CRUD
- [ ] Filtering by status, client, search term

### Task Management
- [ ] Full CRUD
- [ ] Filtering by status, priority

### Invoicing & Payments
- [ ] Invoice CRUD
- [ ] Partial payments against invoices
- [ ] Dynamically computed invoice status and outstanding balance

### Reporting
- [ ] Dashboard endpoint (active projects, pending invoices, monthly revenue, etc.)
- [ ] Revenue / project / invoice reports

### Infrastructure
- [ ] Global exception handling middleware
- [ ] Structured logging (Serilog)
- [ ] Rate limiting
- [ ] Health check endpoint
- [ ] Unit tests
- [ ] Docker support
- [ ] Render deployment

## Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL (local or via Docker)

### Setup

```bash
git clone https://github.com/Rethabile2004/freelanceflow-api.git
cd freelanceflow-api
dotnet restore
```

Update the connection string in `appsettings.Development.json` (not committed — see `.gitignore`) with your local PostgreSQL credentials, then apply migrations:

```bash
dotnet ef database update
```

Run the API:

```bash
dotnet run --project FreelanceFlow.API
```

Swagger UI will be available at `/swagger` once running.

## API Documentation

Full endpoint documentation will be added here as endpoints are completed. In the meantime, the live Swagger UI is the source of truth for available routes.

## License

This project is for portfolio/educational purposes.
