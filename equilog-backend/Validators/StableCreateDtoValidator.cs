using equilog_backend.DTOs.StableDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class StableCreateDtoValidator : AbstractValidator<StableCreateDto>
{
    public StableCreateDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Stable name is required.");
    }
}

