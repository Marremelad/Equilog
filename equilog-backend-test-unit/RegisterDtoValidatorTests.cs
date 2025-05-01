using equilog_backend.DTOs.AuthDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class RegisterDtoValidatorTests
    {
        private readonly RegisterDtoValidator _validator;

        public RegisterDtoValidatorTests()
        {
            _validator = new RegisterDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_RegisterDto()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Pass_When_PhoneNumber_IsNull()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!",
                PhoneNumber = null
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "First name is required.")]
        [InlineData(null, "First name is required.")]
        public void Should_Fail_When_FirstName_IsNullOrEmpty(string firstName, string expectedError)
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = firstName,
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!"
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
            var dto = new RegisterDto
            {
                FirstName = new string('A', 51),
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!"
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
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = lastName,
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!"
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
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = new string('A', 51),
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("Last name cannot exceed 50 characters.");
        }

        [Theory]
        [InlineData("", "Email is required.")]
        [InlineData(null, "Email is required.")]
        public void Should_Fail_When_Email_IsNullOrEmpty(string email, string expectedError)
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                UserName = "johndoe",
                Password = "Password123!"
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
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = longEmail,
                UserName = "johndoe",
                Password = "Password123!"
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
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = email,
                UserName = "johndoe",
                Password = "Password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Invalid email format.");
        }

        [Theory]
        [InlineData("", "Username is required.")]
        [InlineData(null, "Username is required.")]
        public void Should_Fail_When_UserName_IsNullOrEmpty(string userName, string expectedError)
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = userName,
                Password = "Password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_UserName_ExceedsMaxLength()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = new string('a', 31),
                Password = "Password123!"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserName)
                .WithErrorMessage("Email cannot exceed 30 characters.");
        }

        [Theory]
        [InlineData("", "Password is required.")]
        [InlineData(null, "Password is required.")]
        public void Should_Fail_When_Password_IsNullOrEmpty(string password, string expectedError)
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = password
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_Password_TooShort()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Pass1!"  // 6 chars, minimum is 8
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password needs to be at least 8 characters.");
        }

        [Fact]
        public void Should_Fail_When_Password_ExceedsMaxLength()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "Password123!" + new string('a', 101)
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password cannot exceed 100 characters.");
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoUppercase()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe123",
                Password = "password123!",
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoLowercase()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe123",
                Password = "PASSWORD123!",
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoDigit()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe123",
                Password = "Password!",
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Fail_When_Password_HasNoSpecialChar()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe123",
                Password = "Password123",
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Fail_When_PhoneNumber_ExceedsMaxLength()
        {
            // Arrange
            var dto = new RegisterDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                UserName = "johndoe",
                Password = "password123",
                PhoneNumber = new string('1', 21)
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("Phone number cannot exceed 20 characters.");
        }
    }
}
