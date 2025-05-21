using equilog_backend.DTOs.CalendarEventDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class CalendarEventCreateDtoValidator : AbstractValidator<CalendarEventCreateDto>
{
    public CalendarEventCreateDtoValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");

        RuleFor(e => e.StartDateTime)
            .NotEmpty().WithMessage("Start is required.");

        RuleFor(e => e.EndDateTime)
            .NotEmpty().WithMessage("End is required.")
            .GreaterThanOrEqualTo(d => d.StartDateTime).WithMessage("End time must be after start time.");

        RuleFor(e => e.StableIdFk)
            .NotEmpty().WithMessage("Stable id is required.")
            .GreaterThan(0).WithMessage("Stable ID must be greater than 0.");
    }
}


