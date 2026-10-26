<h1>ToDo Task App</h1>
The concept behind the app is simple: it’s a task management application. It features a frontend where, upon logging in, users are taken to a dashboard that allows them to create tasks and receive mobile notifications—one alert 5 minutes before a task is due, and another when the time for that task has elapsed.
<br>

<h1>
 👷‍♂️🚧🚧🚧 Under Construction 🚧🚧🚧👷‍♂️
</h1>
<br>

<h2> 🛠️ Tech Stack </h2>

<div align="left">

| Layer / Category | Technology & Tools |
| :--- | :--- |
| **Backend & API** | ![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=flat&logo=csharp&logoColor=white) • **.NET 10** • **ASP.NET Core (Minimal APIs)** |
| **Database & ORM** | ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-%23316192.svg?style=flat&logo=postgresql&logoColor=white) • **Entity Framework Core** |
| **Authentication** | **ASP.NET Core Identity** • **JWT (HttpOnly Cookies)** |
| **Scheduling & Messaging** | **Quartz.NET** • (RabbitMQ under study for events) |
| **Validation & Patterns** | **FluentValidation** • **Result Pattern** |
| **Documentation & Tests** | **Scalar** (OpenAPI) • **Health Checks** • **xUnit (in progress)** |
| **Observability** | **Serilog** (Console & File Logging with Rolling Interval) |
| **Frontend (Planned)** | ![React](https://img.shields.io/badge/react-%2320232a.svg?style=flat&logo=react&logoColor=%2361DAFB) • **TypeScript** |
| **Architecture** | **Clean Architecture** (Api, Application, Infrastructure) |

</div>
<br>

> **Evolution Note:** Initially, the project used a `BackgroundService` to manage task timing. However, to guarantee millisecond precision and persistence (so scheduled jobs aren't lost if the API restarts), we migrated to **Quartz.NET**. Some things will change—this is a living project.

<br>

<h2>🛠️ Architecture's Explanation</h2>

If you want to see how the app works in an interactive way, click the link below (it will open an HTML file):

<a href="https://atonementandy.github.io/todoapp-architecture/" target="_blank">Architecture Explanation</a>

![TodoApp Diagram](./architecture.svg)

<br>

---

## 🧠 Architecture Decisions & Problem Solving

This section documents the technical challenges faced and the engineering decisions made during development.

### 1. Identity Inheritance Conflict (CS0311)
*   **Problem:** Error when inheriting from `IdentityDbContext<User>`.
*   **Solution:** The `User` entity was adjusted to inherit from `IdentityUser<Guid>`, and `AppDbContext` was configured as `IdentityDbContext<User, IdentityRole<Guid>, Guid>`.
*   **Why?** The old Identity version (`Microsoft.AspNet.Identity`) used `string` for the `Id`. The modern version (`AspNetCore.Identity`) allows custom ID types, but requires the `DbContext` to know exactly which type is used to avoid EF Core conversion conflicts.

### 2. Error Handling with Result Pattern
*   **Problem:** How to return validation errors (e.g., duplicate email, weak password) so the frontend can map each error to the correct field?
*   **Solution:** Implementation of the `Result` and `Error` pattern in the Application layer. No `Exception` is used for business validation flows. The endpoint returns `ProblemDetails` (RFC 7807) with a structured error list.
*   **Why?** `Exceptions` are expensive in performance and unstructured for the frontend. `Result` is the clean and explicit way to return business failures.

### 3. Token Security (JWT in HttpOnly Cookie)
*   **Problem:** Where to store the JWT on the frontend securely?
*   **Solution:** The JWT is generated at login but returned in an **HttpOnly Cookie** (with `Secure` and `SameSite=Strict`).
*   **Why?** `localStorage` is vulnerable to XSS attacks (any malicious script can read the token). The HttpOnly cookie cannot be read via JavaScript. The `SameSite=Strict` flag mitigates CSRF attacks.

### 4. Precise Scheduling (Quartz.NET vs BackgroundService)
*   **Problem:** Trigger an SMS exactly 5 minutes before the task's `DueDate`.
*   **Solution:** Use of Quartz.NET with point-in-time triggers (`StartAt`), replacing the `BackgroundService` with polling (`Task.Delay`).
*   **Why?** Polling generates unnecessary database load and delays up to X seconds. Quartz persists the schedule in the database, wakes up at the exact millisecond, and doesn't lose jobs if the API restarts.

### 5. Validation with FluentValidation
*   **Problem:** Where to put validation rules (valid email, strong password)?
*   **Solution:** FluentValidation in the Application layer, injected into Services.
*   **Why?** Keeps endpoints "dumb" (they only receive and return) and allows testing business rules in isolation, without spinning up the API.

<br>

## 🚧 Roadmap
- [x] ASP.NET Core Identity configuration with `Guid`
- [x] Result Pattern implementation
- [ ] Completion of `TaskReminderJob` and Twilio/Zenvia (SMS) integration
- [ ] Task CRUD Endpoints (with JWT authentication)
- [ ] React + TypeScript frontend
- [ ] Unit tests (xUnit)

<br>

## 🚀 How to run the project

### Prerequisites
- .NET 10 SDK
- PostgreSQL

### Setup
1. Clone the repository: `git clone https://github.com/AtonementAndy/todoapp.git`
2. Configure `appsettings.json` with the PostgreSQL connection string.
3. Run the Quartz SQL script (to create the `QRTZ_*` tables).
4. Run migrations: `dotnet ef database update`
5. Run the API: `dotnet run --project src/TodoApp.Api`

<br>

## 👨‍💻 Author
**Andy Santos**
[LinkedIn](https://www.linkedin.com/in/andre-saantos/) | [GitHub](https://github.com/AtonementAndy)
