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

        RuleFor(h => h.Color)
            .MaximumLength(50).When(h => h.Color != null);

        RuleFor(h => h.Breed)
            .MaximumLength(50).When(h => h.Breed != null);
    }
}
