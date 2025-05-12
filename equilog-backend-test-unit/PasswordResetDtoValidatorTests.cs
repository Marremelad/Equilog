using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.DTOs.PasswordResetDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class PasswordResetDtoValidatorTests
    {
        private readonly PasswordResetDtoValidator _validator;

        public PasswordResetDtoValidatorTests()
        {
            _validator = new PasswordResetDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_PasswordResetDto()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_Email_IsInvalid()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "invalid-email",
                NewPassword = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Fail_When_Email_IsEmpty()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "",
                NewPassword = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Fail_When_Password_TooShort()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "Short1!",  // 7 characters
                ConfirmPassword = "Short1!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Fail_When_Password_TooLong()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = new string('A', 101) + "a1!",  // 104 characters
                ConfirmPassword = new string('A', 101) + "a1!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoUppercase()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "password123!",
                ConfirmPassword = "password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoLowercase()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "PASSWORD123!",
                ConfirmPassword = "PASSWORD123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoDigit()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "Password!",
                ConfirmPassword = "Password!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoSpecialChar()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "Password123",
                ConfirmPassword = "Password123"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Fail_When_Password_IsEmpty()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "",
                ConfirmPassword = ""
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.NewPassword);
        }

        [Fact]
        public void Should_Fail_When_Passwords_DoNotMatch()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "Password123!",
                ConfirmPassword = "DifferentPassword123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        [Fact]
        public void Should_Fail_When_ConfirmPassword_IsEmpty()
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = "test@example.com",
                NewPassword = "Password123!",
                ConfirmPassword = ""
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        [Theory]
        [InlineData("test@example.com", "Password123!", "Password123!")]
        [InlineData("user.name+tag@example.co.uk", "Abcdef1@", "Abcdef1@")]
        [InlineData("name@host.io", "P@55w0rdXYZ", "P@55w0rdXYZ")]
        public void Should_Pass_When_AllInputsAreValid(string email, string password, string confirmPassword)
        {
            // Arrange
            var dto = new PasswordResetDto
            {
                Email = email,
                NewPassword = password,
                ConfirmPassword = confirmPassword
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
