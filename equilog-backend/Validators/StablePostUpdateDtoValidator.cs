using equilog_backend.DTOs.StablePostDTOs;
using FluentValidation;

namespace equilog_backend.Validators
{
    public class StablePostUpdateDtoValidator : AbstractValidator<StablePostUpdateDto>
    {
        public StablePostUpdateDtoValidator()
        {
            RuleFor(sp => sp.Id)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Stable post ID must be greater than 0.");

            RuleFor(sp => sp.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(510).WithMessage("Title cannot exceed 510 characters.");

            RuleFor(sp => sp.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(4094).WithMessage("Content cannot exceed 4094 characters.");
        }
    }
}
