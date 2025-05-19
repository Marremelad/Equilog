using equilog_backend.DTOs.HorseDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class HorseCreateDtoValidator : AbstractValidator<HorseCreateDto>
{
	public HorseCreateDtoValidator()
	{
		RuleFor(h => h.Name)
			.NotEmpty().WithMessage("Name is required.")
			.MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

		RuleFor(h => h.Age)
			.LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
			.When(h => h.Age.HasValue)
			.WithMessage("Age must be today or a past date.");

		RuleFor(h => h.Color)
			.MaximumLength(50).When(h => h.Color != null)
			.WithMessage("Color cannot exceed 50 characters");

		RuleFor(h => h.Breed)
			.MaximumLength(50).When(h => h.Breed != null)
			.WithMessage("Breed cannot exceed 50 characters");
	}
}
