namespace TodoApp.Application.DTOs
{
    public record AddTaskDto(
        string Title,
        string Description,
        bool IsCompleted,
        DateTime? DueDate,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        bool IsDeleted
    );
}
