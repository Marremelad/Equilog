using equilog_backend.DTOs.CommentCompositionDTOs;
using FluentValidation;

namespace equilog_backend.Validators
{
	public class CommentCompositionCreateDtoValidator : AbstractValidator<CommentCompositionCreateDto>
	{
		public CommentCompositionCreateDtoValidator()
		{
			RuleFor(c => c.UserId)
				.NotEmpty()
				.GreaterThan(0).WithMessage("User ID must be greater than 0.");

			RuleFor(c => c.StablePostId)
				.NotEmpty()
				.GreaterThan(0).WithMessage("User ID must be greater than 0.");

			RuleFor(c => c.Comment)
				.SetValidator(new CommentCreateDtoValidator());
		}
	}
}
