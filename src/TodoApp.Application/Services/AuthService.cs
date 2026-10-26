using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TodoApp.Application.Common;
using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Application.Models.Entities;

namespace TodoApp.Application.Services
{
    public class AuthService(
        UserManager<User> userManager,
        IValidator<RegisterRequest> validator) : IAuthService
    {
        public async Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => new Error(e.PropertyName, e.ErrorMessage))
                    .ToList();

                return Result.Failure(errors);
            }

            var user = User.Create(
                request.Name,
                request.Surname,
                request.PhoneNumber,
                request.Email
            );

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors =  result.Errors
                    .Select(e => new Error(e.Code, e.Description))
                    .ToList();

                return Result.Failure(errors);
            }

            return Result.Success();
        }
    }
}
