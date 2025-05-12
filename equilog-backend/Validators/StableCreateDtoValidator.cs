using equilog_backend.DTOs.StableDTOs;
using FluentValidation;

namespace equilog_backend.Validators;
public class StableCreateDtoValidator : AbstractValidator<StableCreateDto>
{
    public StableCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required")
            .MaximumLength(50).WithMessage("Type cannot exceed 50 characters");

        RuleFor(x => x.County)
            .NotEmpty().WithMessage("County is required")
            .MaximumLength(50).WithMessage("County cannot exceed 50 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(50).WithMessage("Address cannot exceed 50 characters");

        RuleFor(x => x.PostCode)
            .MaximumLength(5).WithMessage("PostCode cannot exceed 5 characters")
            .Matches(@"^\d+$").WithMessage("Only digits are allowed.");

        RuleFor(x => x.BoxCount)
            .NotEmpty().WithMessage("Box Count is required")
            .GreaterThan(0).WithMessage("Box Count must be a positive number")
            .LessThan(1000000).WithMessage("Box Count must be less than 1000000");
    }
}

