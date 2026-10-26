using TodoApp.Application.Common;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    }
}
