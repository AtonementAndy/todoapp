using TodoApp.Application.Common;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces
{
    public interface ITaskService
    {
        Task<Result<TaskDto>> AddAsync(CreateTaskRequest request, CancellationToken cancellationToken);
    }
}
