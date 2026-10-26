namespace TodoApp.Application.DTOs
{
    public record CreateTaskRequest(
        string Title,
        string Description,
        Guid UserId,
        DateTime DueDate
    );
}
