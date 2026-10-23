using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Models.Entities;

namespace TodoApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);

                builder.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                builder.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(256);

                builder.HasIndex(t => t.Email)
                    .IsUnique();

                builder.Property(u => u.PasswordHash)
                    .IsRequired();

                builder.Property(u => u.CreatedAt)
                    .IsRequired();

                builder.HasMany(u => u.Tasks)
                    .WithOne(t => t.User)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TodoTask>(builder =>
            {
                builder.HasKey(t => t.Id);

                builder.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(u => u.Description)
                    .HasMaxLength(500);

                builder.Property(u => u.Priority)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                builder.Property(t => t.CreatedAt)
                    .IsRequired();
            });
        }
    }
}
