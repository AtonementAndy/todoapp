using TodoApp.Application.Models.Enum;

namespace TodoApp.Application.Models.Entities
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Key
        public Guid UserId { get; set; }

        // Navigation
        public User User { get; set; } = default!;

        private TodoTask() { }

        public static TodoTask Create(
            string title,
            string? description,
            Guid userId,
            Priority priority = Priority.Medium,
            DateTime? dueDate = null)
        {
            return new TodoTask
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Priority = priority,
                DueDate = dueDate,
                UserId = userId,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }
    }
}
