using equilog_backend.DTOs.HorseCompositionDTOs;
using FluentValidation;

namespace equilog_backend.Validators
{
    public class HorseCompositionCreateDtoValidator : AbstractValidator<HorseCompositionCreateDto>
    {
        public HorseCompositionCreateDtoValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(c => c.StableId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Stable ID must be greater than 0.");

            RuleFor(c => c.Horse)
                .SetValidator(new HorseCreateDtoValidator());
        }
    }
}
