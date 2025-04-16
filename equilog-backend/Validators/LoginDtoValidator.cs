using equilog_backend.DTOs.AuthDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(l => l.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(254).WithMessage("Email cannot exceed 254 characters.")
            .EmailAddress().WithMessage("Not a valid email format.");

        RuleFor(l => l.Password)
            .NotEmpty().WithMessage("Please enter a password.");
    }
}
