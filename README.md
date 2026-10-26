<h1>ToDo Task App</h1>
The concept behind the app is simple: it’s a task management application. It features a frontend where, upon logging in, users are taken to a dashboard that allows them to create tasks and receive mobile notifications—one alert 5 minutes before a task is due, and another when the time for that task has elapsed.
<br>

<h1>
 👷‍♂️🚧🚧🚧 Under Construction 🚧🚧🚧👷‍♂️
</h1>
<br>

<h2> 🛠️ Tech Stack </h2>

<div align="left">

| Layer / Category | Tecnology & Tools |
| :--- | :--- |
| **Backend & API** | ![C#](https://img.shields.io/badge/C%23-%23239120.svg?style=flat&logo=csharp&logoColor=white) • **.NET 10** • **ASP.NET Core (Minimal APIs)** |
| **Database & ORM** | ![PostgreSQL](https://img.shields.io/badge/PostgreSQL-%23316192.svg?style=flat&logo=postgresql&logoColor=white) • **Entity Framework Core** |
| **MessageBroker** | **RabbitMQ** • **BackgroundService** (.NET) |
| **Documentation & Tests** | **Scalar** (OpenAPI / ScalarApiReference) • **Health Checks** |
| **Observability** | **Serilog** (Console & File Logging com Rolling Interval) |
| **Frontend (Planejado)** | ![React](https://img.shields.io/badge/react-%2320232a.svg?style=flat&logo=react&logoColor=%2361DAFB) • **TypeScript** |
| **3 Layers API** | **Decoupling the app** |

This is the tech stack I’ve planned, but some things will change. Why? When a user creates a new task, the `BackgroundService` won’t be precise enough in completing it. After doing some research, I found that Quartz.NET handles exactly this. So, in the near future, I’ll replace the `BackgroundService` with Quartz.NET.

</div>
<br>

<h2>🛠️ Achitecture's Explanation</h2>

If you want to see how the app works in a interactive way, click in the link below, it will open a html file!

<a href="https://atonementandy.github.io/todoapp-architecture/" target="_blank">Architecture Explanation</a>

![Diagrama TodoApp](./architecture.svg)


