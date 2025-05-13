using equilog_backend.DTOs.StableCompositionDtos;
using FluentValidation;

namespace equilog_backend.Validators
{
    public class StableCompositionCreateDtoValidator : AbstractValidator<StableCompositionCreateDto>
    {
        public StableCompositionCreateDtoValidator()
        {
            RuleFor(c => c.UserId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(c => c.Stable)
                .SetValidator(new StableCreateDtoValidator());
        }
    }
}
