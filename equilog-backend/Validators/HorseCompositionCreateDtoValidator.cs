using equilog_backend.DTOs.HorseCompositionDTOs;
using FluentValidation;

namespace equilog_backend.Validators
{
    public class HorseCompositionCreateDtoValidator : AbstractValidator<HorseCompositionCreateDto>
    {
        public HorseCompositionCreateDtoValidator()
        {
            RuleFor(e => e.UserId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(e => e.StableId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Stable ID must be greater than 0.");

            RuleFor(x => x.Horse)
                .SetValidator(new HorseCreateDtoValidator());
        }
    }
}
