using TodoApp.Application.DTOs;
using TodoApp.Application.Models.Entities;

namespace TodoApp.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskDto>> GetTodoTasksAsync(CancellationToken cancellationToken);
        Task<TaskDto> GetTaskById(Guid id, CancellationToken cancellationToken);
        Task<AddTaskDto> AddAsync(TodoTask newTask, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(Guid id, TodoTask taskToUpdate, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
