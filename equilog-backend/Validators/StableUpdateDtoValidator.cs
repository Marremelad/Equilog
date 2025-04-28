using equilog_backend.DTOs.StableDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class StableUpdateDtoValidator : AbstractValidator<StableUpdateDto>
{
    public StableUpdateDtoValidator()
    {
        RuleFor(s => s.Id)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Stable ID must be greater than 0.");

        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Stable name is required.");
    }
}

