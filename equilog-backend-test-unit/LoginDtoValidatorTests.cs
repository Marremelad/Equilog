using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class LoginDtoValidatorTests
    {
        private readonly LoginDtoValidator _validator;

        public LoginDtoValidatorTests()
        {
            _validator = new LoginDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_LoginDto()
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "test@example.com",
                Password = "password123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "Email is required.")]
        [InlineData(null, "Email is required.")]
        public void Should_Fail_When_Email_IsNullOrEmpty(string email, string expectedError)
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = email,
                Password = "password123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_Email_ExceedsMaxLength()
        {
            // Arrange
            var longEmail = new string('a', 245) + "@example.com"; // 245 + 12 = 257 chars
            var dto = new LoginDto
            {
                Email = longEmail,
                Password = "password123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Email cannot exceed 254 characters.");
        }

        [Theory]
        [InlineData("notanemail")]
        [InlineData("notanemail@")]
        [InlineData("@example.com")]
        public void Should_Fail_When_Email_HasInvalidFormat(string email)
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = email,
                Password = "password123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Not a valid email format.");
        }

        [Theory]
        [InlineData("", "Please enter a password.")]
        [InlineData(null, "Please enter a password.")]
        public void Should_Fail_When_Password_IsNullOrEmpty(string password, string expectedError)
        {
            // Arrange
            var dto = new LoginDto
            {
                Email = "test@example.com",
                Password = password
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(expectedError);
        }
    }
}
