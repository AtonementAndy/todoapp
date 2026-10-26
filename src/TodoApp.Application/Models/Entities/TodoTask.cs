namespace TodoApp.Application.Models.Entities
{
    public class TodoTask
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public bool IsCompleted { get; private set; }
        public DateTime? DueDate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }

        // Foreign Key
        public Guid UserId { get; private set; }
        // Navigation
        public User User { get; private set; } = default!;

        private TodoTask() { }

        public static TodoTask Create(
            string title,
            string description,
            Guid userId,
            DateTime? dueDate = null)
        {
            return new TodoTask
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                DueDate = dueDate,
                UserId = userId,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        public void IsDeleteDetailUpdate(bool isDeleted) => IsDeleted = isDeleted;
        public void DueDateDetailUpdate(DateTime? dueDate) => DueDate = dueDate;
        public void UpdateDetails(
            string title,
            string description,
            DateTime? dueDate,
            DateTime updatedAt
        )
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            UpdatedAt = updatedAt;
            IsCompleted = false;
            IsDeleted = false;
        }
    }
}
