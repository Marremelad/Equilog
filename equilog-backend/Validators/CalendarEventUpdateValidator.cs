using equilog_backend.DTOs.CalendarEventDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class CalendarEventUpdateValidator : AbstractValidator<CalendarEventUpdateDto>
{
	public CalendarEventUpdateValidator()
	{
		RuleFor(e => e.Id)
			.NotEmpty()
			.GreaterThan(0).WithMessage("Calendar ID must be greater than 0.");

		RuleFor(e => e.Title)
			.NotEmpty().WithMessage("Title is required.")
			.MaximumLength(50).WithMessage("Title cannot exceed 50 characters.");

		RuleFor(e => e.StartDateTime)
			.NotEmpty().WithMessage("Start is required.");

		RuleFor(e => e.EndDateTime)
			.NotEmpty().WithMessage("End is required.")
			.GreaterThanOrEqualTo(d => d.StartDateTime).WithMessage("End time must be after start time.");
	}
}


