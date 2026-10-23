namespace TodoApp.Application.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateddAt { get; set; }
        public bool IsDeleted { get; set; }

        private User() { }

        public static User Create(
            string name,
            string email,
            string passwordHash)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Navigation
        public ICollection<TodoTask> Tasks { get; set; } = [];
    }
}
