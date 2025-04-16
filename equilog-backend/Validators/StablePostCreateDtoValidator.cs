using equilog_backend.DTOs.StablePostDTOs;
using FluentValidation;

namespace equilog_backend.Validators
{
    public class StablePostCreateDtoValidator : AbstractValidator<StablePostCreateDto>
    {
        public StablePostCreateDtoValidator()
        {
            RuleFor(x => x.UserIdFk)
                .NotEmpty()
                .GreaterThan(0).WithMessage("User ID must be greater than 0");

            RuleFor(x => x.StableIdFk)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Stable ID must be greater than 0");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(510).WithMessage("Title cannot exceed 510 characters");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(4094).WithMessage("Content cannot exceed 4094 characters");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required");

            RuleFor(x => x.IsPinned)
                .NotNull().WithMessage("IsPinned status is required");
        }
    }
}