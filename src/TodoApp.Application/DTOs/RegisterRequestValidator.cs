using FluentValidation;

namespace TodoApp.Application.DTOs
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-mail is required")
                .EmailAddress().WithMessage("Invalid e-mail format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must have 8 caracters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phonenumber is required to send the SMS.");
        }
    }
}
