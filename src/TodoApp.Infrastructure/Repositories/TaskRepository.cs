using Microsoft.EntityFrameworkCore;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Models.Entities;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    internal class TaskRepository(AppDbContext context) : ITaskRepository
    {
        public async Task<IEnumerable<TaskDto>> GetTodoTasksAsync(CancellationToken cancellationToken)
        {
            return await context.TodoTasks
                .Where(t => t.IsDeleted == false && t.IsCompleted == false)
                .Select(task => new TaskDto
                (
                    task.Id,
                    task.Title,
                    task.Description,
                    task.IsCompleted,
                    task.DueDate,
                    task.CreatedAt,
                    task.UpdatedAt,
                    task.IsDeleted,
                    task.User.PhoneNumber ?? "PhoneNumber not found."
                ))
                .ToListAsync(cancellationToken);
        }

        public async Task<TaskDto> GetTaskById(Guid id, CancellationToken cancellationToken)
        {
            var task = await context.TodoTasks.FindAsync([id], cancellationToken)
                ?? throw new NullReferenceException("There's no such task in the database.");

            return new TaskDto(
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.DueDate,
                task.CreatedAt,
                task.UpdatedAt,
                task.IsDeleted,
                task.User.PhoneNumber ?? "PhoneNumber not found."
            );
        }

        public async Task<AddTaskDto> AddAsync(TodoTask newTask, CancellationToken cancellationToken)
        {
            await context.TodoTasks.AddAsync(newTask, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return new AddTaskDto(
                newTask.Title,
                newTask.Description,
                newTask.IsCompleted,
                newTask.DueDate,
                newTask.CreatedAt,
                newTask.UpdatedAt,
                newTask.IsDeleted
            );
        }
        
        public async Task<bool> UpdateAsync(Guid id, TodoTask taskToUpdate, CancellationToken cancellationToken)
        {
            var task = await context.TodoTasks.FindAsync([id], cancellationToken)
                ?? throw new NullReferenceException("There's no such task in the database.");

            task.UpdateDetails(
                taskToUpdate.Title,
                taskToUpdate.Description,
                taskToUpdate.DueDate,
                DateTime.UtcNow
            );

            await context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var task = await context.TodoTasks.FindAsync([id], cancellationToken)
                ?? throw new NullReferenceException("There's no such task in the database.");

            task.IsDeleteDetailUpdate(true);
            await context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
