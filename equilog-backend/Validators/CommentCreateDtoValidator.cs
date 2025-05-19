using equilog_backend.DTOs.CommentDTOs;
using FluentValidation;
namespace equilog_backend.Validators
{
	public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
	{
		public CommentCreateDtoValidator()
		{
			RuleFor(c => c.Content)
				.NotEmpty().WithMessage("Content is required.")
				.MaximumLength(4094).WithMessage("Content cannot exceed 4094 characters.");
		}
	}
}
