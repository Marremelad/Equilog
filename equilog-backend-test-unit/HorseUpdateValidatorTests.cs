using equilog_backend.DTOs.HorseDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class HorseUpdateDtoValidatorTests
    {
        private readonly HorseUpdateDtoValidator _validator;

        public HorseUpdateDtoValidatorTests()
        {
            _validator = new HorseUpdateDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_HorseUpdateDto()
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = "Thunder",
                Age = DateOnly.FromDateTime(DateTime.Today.AddYears(-5)),
                Color = "Bay",
                Breed = "Arabian"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Pass_When_Optional_Fields_Are_Null()
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = "Thunder",
                Age = null,
                Color = null,
                Breed = null
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Fail_When_Id_NotGreaterThanZero(int id)
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = id,
                Name = "Thunder"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Horse ID must be greater than 0.");
        }

        [Theory]
        [InlineData("", "Name is required.")]
        [InlineData(null, "Name is required.")]
        public void Should_Fail_When_Name_IsNullOrEmpty(string name, string expectedError)
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = name
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_Name_ExceedsMaxLength()
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = new string('A', 51) // 51 characters, exceeding the 50 character limit
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Name cannot exceed 50 characters.");
        }

        [Fact]
        public void Should_Pass_When_Age_IsInPast()
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = "Thunder",
                Age = DateOnly.FromDateTime(DateTime.Today.AddDays(-1))
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Age);
        }

        [Fact]
        public void Should_Fail_When_Age_IsInFuture()
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = "Thunder",
                Age = DateOnly.FromDateTime(DateTime.Today).AddDays(1)
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Age)
                .WithErrorMessage("Age must be today or a past date.");
        }

        [Fact]
        public void Should_Fail_When_Color_ExceedsMaxLength()
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = "Thunder",
                Color = new string('A', 51) // 51 characters, exceeding the 50 character limit
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Color);
        }

        [Fact]
        public void Should_Fail_When_Breed_ExceedsMaxLength()
        {
            // Arrange
            var dto = new HorseUpdateDto
            {
                Id = 1,
                Name = "Thunder",
                Breed = new string('A', 51) // 51 characters, exceeding the 50 character limit
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Breed);
        }
    }

}
