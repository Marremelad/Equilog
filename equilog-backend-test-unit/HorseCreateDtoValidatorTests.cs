using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class HorseCreateDtoValidatorTests
    {
        private readonly HorseCreateDtoValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            var dto = new HorseCreateDto
            {
                Name = "",
                Color = "Brown",
                Breed = "Arabian"
            };

            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Too_Long()
        {
            var dto = new HorseCreateDto
            {
                Name = new string('A', 51),
                Color = "Brown",
                Breed = "Arabian"
            };

            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Color_Too_Long()
        {
            var dto = new HorseCreateDto
            {
                Name = "Comet",
                Color = new string('B', 51),
                Breed = "Arabian"
            };

            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Color);
        }

        [Fact]
        public void Should_Have_Error_When_Breed_Too_Long()
        {
            var dto = new HorseCreateDto
            {
                Name = "Comet",
                Color = "Brown",
                Breed = new string('C', 51)
            };

            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(x => x.Breed);
        }

        [Fact]
        public void Should_Pass_With_Valid_Fields()
        {
            var dto = new HorseCreateDto
            {
                Name = "Comet",
                Color = "Brown",
                Breed = "Arabian",
                Age = new DateOnly(2018, 1, 1)
            };

            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
