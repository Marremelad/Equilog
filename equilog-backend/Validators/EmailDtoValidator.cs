using equilog_backend.DTOs.EmailDTOs;
using FluentValidation;

namespace equilog_backend.Validators;

public class EmailDtoValidator : AbstractValidator<EmailDto>
{
    public EmailDtoValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(254).WithMessage("Email cannot exceed 254 characters.");
    }
}