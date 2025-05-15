using equilog_backend.DTOs.PasswordDTOs;
using FluentValidation;

namespace equilog_backend.Validators
{
	public class PasswordChangeDtoValidator : AbstractValidator<PasswordChangeDto>
	{
		public PasswordChangeDtoValidator()
		{
			RuleFor(x => x.UserId)
				.NotEmpty()
				.GreaterThan(0).WithMessage("User ID must be greater than 0.");

			RuleFor(x => x.NewPassword)
				.NotEmpty().WithMessage("New password is required.")
				.MinimumLength(8).WithMessage("Password must be at least 8 characters.")
				.MaximumLength(100).WithMessage("Password cannot exceed 100 characters.")
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain at least one number.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage("Password confirmation is required.")
				.Equal(x => x.NewPassword).WithMessage("Passwords do not match.");
		}
	}
}
