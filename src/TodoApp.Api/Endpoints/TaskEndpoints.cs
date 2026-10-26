using TodoApp.Api.Extensions;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;

namespace TodoApp.Api.Endpoints
{
    public static class TaskEndpoints
    {
        public static void Endpoints(this WebApplication app)
        {
            app.MapPost("/api/auth/register", async (
                RegisterRequest request,
                IAuthService authService,
                CancellationToken cancellationToken) =>
            {
                var result = await authService.RegisterAsync(request, cancellationToken);

                if (result.IsFailure)
                {
                    return result.ToProblemDetails();
                }

                return Results.Created();

            }).AllowAnonymous();

            app.MapPost("/api/tasks", async (
                CreateTaskRequest request,
                ITaskService taskService,
                CancellationToken cancellationToken) =>
            {
                var result = await taskService.AddAsync(request, cancellationToken);
                return result.IsSuccess
                    ? Results.Created($"/api/tasks/{result.Value.Id}", result.Value)
                    : result.ToProblemDetails();
            })
             .RequireAuthorization(); // Protegido por JWT
        }
    }
}
