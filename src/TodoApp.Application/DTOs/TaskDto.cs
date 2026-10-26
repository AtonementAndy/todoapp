namespace TodoApp.Application.DTOs
{
    public record TaskDto(
        Guid Id,
        string Title,
        string Description,
        bool IsCompleted,
        DateTime? DueDate,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        bool IsDeleted,
        string UserPhoneNumber
    );
}
