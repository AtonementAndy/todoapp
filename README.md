# 🚀 TodoApp - API de Tarefas com Agendamento

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
| **Scheduling & Messaging** | **Quartz.NET** • (RabbitMQ em estudo para eventos) |
| **Validation & Patterns** | **FluentValidation** • **Result Pattern** |
| **Documentation & Tests** | **Scalar** (OpenAPI) • **Health Checks** • **xUnit (em progresso)** |
| **Observability** | **Serilog** (Console & File Logging) |
| **Frontend (Planejado)** | ![React](https://img.shields.io/badge/react-%2320232a.svg?style=flat&logo=react&logoColor=%2361DAFB) • **TypeScript** |
| **Architecture** | **Clean Architecture** (Api, Application, Infrastructure) |

</div>
<br>

> **Nota sobre Evolução:** Inicialmente, o projeto usava um `BackgroundService` para gerenciar o tempo das tarefas. No entanto, para garantir precisão milimétrica e persistência (para não perder o agendamento se a API reiniciar), migramos para o **Quartz.NET**. 

<br>

<h2>🛠️ Architecture Explanation</h2>

If you want to see how the app works in an interactive way, click the link below (it will open an HTML file):

<a href="https://atonementandy.github.io/todoapp-architecture/" target="_blank">Architecture Explanation</a>

![Diagrama TodoApp](./architecture.svg)

<br>

---

## 🧠 Decisões de Arquitetura e Soluções de Problemas

Esta seção documenta os desafios técnicos enfrentados e as decisões de engenharia tomadas durante o desenvolvimento.

### 1. Conflito de Herança no Identity (CS0311)
*   **Problema:** Erro ao herdar de `IdentityDbContext<User>`.
*   **Solução:** A entidade `User` foi ajustada para herdar de `IdentityUser<Guid>`, e o `AppDbContext` foi configurado como `IdentityDbContext<User, IdentityRole<Guid>, Guid>`.
*   **Por quê?** A versão antiga do Identity (`Microsoft.AspNet.Identity`) usava `string` para o `Id`. A versão moderna (`AspNetCore.Identity`) permite customizar o tipo do ID, mas exige que o `DbContext` saiba exatamente qual tipo está sendo usado para evitar conflitos de conversão no EF Core.

### 2. Tratamento de Erros com Result Pattern
*   **Problema:** Como retornar erros de validação (ex: e-mail duplicado, senha fraca) de forma que o frontend consiga mapear cada erro ao campo correto?
*   **Solução:** Implementação do padrão `Result` e `Error` na camada Application. Nenhuma `Exception` é usada para fluxos de validação de negócio. O endpoint retorna um `ProblemDetails` (RFC 7807) com a lista de erros estruturada.
*   **Por quê?** `Exceptions` são caras em performance e não são estruturadas para o frontend. O `Result` é a forma limpa e explícita de devolver falhas de negócio.

### 3. Segurança de Token (JWT em HttpOnly Cookie)
*   **Problema:** Onde armazenar o JWT no frontend de forma segura?
*   **Solução:** O JWT é gerado no login, mas devolvido em um **HttpOnly Cookie** (com `Secure` e `SameSite=Strict`).
*   **Por quê?** O `localStorage` é vulnerável a ataques XSS (qualquer script malicioso pode ler o token). O cookie HttpOnly não pode ser lido via JavaScript. A flag `SameSite=Strict` mitiga ataques CSRF.

### 4. Agendamento Preciso (Quartz.NET vs BackgroundService)
*   **Problema:** Disparar um SMS exatamente 5 minutos antes do `DueDate` da tarefa.
*   **Solução:** Uso do Quartz.NET com triggers pontuais (`StartAt`), substituindo o `BackgroundService` com polling (`Task.Delay`).
*   **Por quê?** O polling gera carga desnecessária no banco e atraso de até X segundos. O Quartz persiste a agenda no banco, acorda no milissegundo exato e não perde os jobs se a API reiniciar.

### 5. Validação com FluentValidation
*   **Problema:** Onde colocar as regras de validação (e-mail válido, senha forte)?
*   **Solução:** FluentValidation na camada Application, injetado nos Services.
*   **Por quê?** Mantém os endpoints "burros" (só recebem e devolvem) e permite testar as regras de negócio de forma isolada, sem subir a API.

<br>

## 🚧 Roadmap
- [x] Configuração do ASP.NET Core Identity com `Guid`
- [x] Implementação do Result Pattern
- [ ] Conclusão do `TaskReminderJob` e integração com Twilio/Zenvia (SMS)
- [ ] Endpoints CRUD de Tasks (com autenticação JWT)
- [ ] Frontend em React + TypeScript
- [ ] Testes unitários (xUnit)

<br>

## 🚀 Como rodar o projeto

### Pré-requisitos
- .NET 10 SDK
- PostgreSQL

### Configuração
1. Clone o repositório: `git clone https://github.com/AtonementAndy/todoapp.git`
2. Configure a `appsettings.json` com a connection string do PostgreSQL.
3. Rode o script SQL do Quartz (para criar as tabelas `QRTZ_*`).
4. Rode as migrations: `dotnet ef database update`
5. Rode a API: `dotnet run --project src/TodoApp.Api`

<br>

## 👨‍💻 Autor
**Andy Santos**
[LinkedIn](https://www.linkedin.com/in/andre-saantos/) | [GitHub](https://github.com/AtonementAndy)
