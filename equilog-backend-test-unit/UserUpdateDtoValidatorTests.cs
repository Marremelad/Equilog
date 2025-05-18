using equilog_backend.DTOs.UserDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class UserUpdateDtoValidatorTests
    {
        private readonly UserUpdateDtoValidator _validator;

        public UserUpdateDtoValidatorTests()
        {
            _validator = new UserUpdateDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_UserUpdateDto()
        {
            // Arrange
            var dto = new UserUpdateDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "test@test.com"
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
            var dto = new UserUpdateDto
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = "test@test.com"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("User ID must be greater than 0.");
        }

        [Theory]
        [InlineData("", "First name is required.")]
        [InlineData(null, "First name is required.")]
        public void Should_Fail_When_FirstName_IsNullOrEmpty(string firstName, string expectedError)
        {
            // Arrange
            var dto = new UserUpdateDto
            {
                Id = 1,
                FirstName = firstName,
                LastName = "Doe",
                Email = "test@test.com"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_FirstName_ExceedsMaxLength()
        {
            // Arrange
            var dto = new UserUpdateDto
            {
                Id = 1,
                FirstName = new string('A', 51), // 51 characters, exceeding the 50 character limit
                LastName = "Doe",
                Email = "test@test.com"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("First name cannot exceed 50 characters.");
        }

        [Theory]
        [InlineData("", "Last name is required.")]
        [InlineData(null, "Last name is required.")]
        public void Should_Fail_When_LastName_IsNullOrEmpty(string lastName, string expectedError)
        {
            // Arrange
            var dto = new UserUpdateDto
            {
                Id = 1,
                FirstName = "John",
                LastName = lastName,
                Email = "test@test.com"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_LastName_ExceedsMaxLength()
        {
            // Arrange
            var dto = new UserUpdateDto
            {
                Id = 1,
                FirstName = "John",
                LastName = new string('A', 51), // 51 characters, exceeding the 50-character limit
                Email = "test@test.com" 
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name cannot exceed 50 characters.");
        }
    }
}
