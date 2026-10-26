using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Models.Entities;

namespace TodoApp.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(builder =>
            {
                builder.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(150);

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

                builder.Property(t => t.CreatedAt)
                    .IsRequired();
            });
        }
    }
}
