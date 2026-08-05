using System.Linq;
using FluentValidation;
using HRMS.Application.DTOs;

namespace HRMS.Application.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 100).WithMessage("Username must be between 3 and 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(12).WithMessage("Password must be at least 12 characters.")
                .Must(HasUpperCase).WithMessage("Password must contain at least one uppercase letter.")
                .Must(HasLowerCase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(HasDigit).WithMessage("Password must contain at least one number.")
                .Must(HasSpecialChar).WithMessage("Password must contain at least one special character.");
        }

        private bool HasUpperCase(string pw) => pw != null && pw.Any(char.IsUpper);
        private bool HasLowerCase(string pw) => pw != null && pw.Any(char.IsLower);
        private bool HasDigit(string pw) => pw != null && pw.Any(char.IsDigit);
        private bool HasSpecialChar(string pw) => pw != null && pw.Any(c => !char.IsLetterOrDigit(c));
    }
}
