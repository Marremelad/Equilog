using equilog_backend.DTOs.AuthDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(r => r.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(254).WithMessage("Email cannot exceed 254 characters.");

        RuleFor(r => r.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(30).WithMessage("Email cannot exceed 30 characters.");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password needs to be at least 8 characters.")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.");

        RuleFor(r => r.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
    }
}
