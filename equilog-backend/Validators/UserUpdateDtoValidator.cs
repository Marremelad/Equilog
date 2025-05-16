using equilog_backend.DTOs.UserDTOs;
using FluentValidation;

namespace equilog_backend.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(u => u.Id)
                .NotEmpty()
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(254).WithMessage("Email cannot exceed 254 characters.");

            RuleFor(u => u.EmergencyContact)
                .MaximumLength(254).WithMessage("Emergency contact cannot exceed 254 characters.");

            RuleFor(u => u.CoreInformation)
                .MaximumLength(254).WithMessage("Core information cannot exceed 254 characters.");

            RuleFor(u => u.Description)
                .MaximumLength(254).WithMessage("Description cannot exceed 254 characters.");

            RuleFor(u => u.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
        }
    }
}
