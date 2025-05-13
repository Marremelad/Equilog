using equilog_backend.DTOs.HorseDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class HorseUpdateDtoValidator : AbstractValidator<HorseUpdateDto>
{
    public HorseUpdateDtoValidator()
    {
        RuleFor(h => h.Id)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Horse ID must be greater than 0.");

        RuleFor(h => h.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(h => h.Age)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(h => h.Age.HasValue)
            .WithMessage("Age must be today or a past date.");

        RuleFor(h => h.Color)
            .MaximumLength(50).When(h => h.Color != null);

        RuleFor(h => h.Breed)
            .MaximumLength(50).When(h => h.Breed != null);
    }
}
