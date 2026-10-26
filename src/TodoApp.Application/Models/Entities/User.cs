using Microsoft.AspNetCore.Identity;

namespace TodoApp.Application.Models.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; private set; } = string.Empty;
        public string Surname { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdateddAt { get; private set; }
        public bool IsDeleted { get; private set; }

        private User() { }

        public static User Create(
            string name,
            string surname,
            string phoneNumber,
            string email)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Surname = surname,
                Email = email,
                UserName = email,
                PhoneNumber = phoneNumber,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Navigation
        public ICollection<TodoTask> Tasks { get; set; } = [];
    }
}
