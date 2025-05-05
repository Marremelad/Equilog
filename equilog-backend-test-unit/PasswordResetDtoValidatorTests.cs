using equilog_backend.DTOs.PasswordDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit;

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
            NewPassword = "Password123!",
            ConfirmPassword = "Password123!",
            Token = "SomeToken"
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
            NewPassword = "Password123!",
            ConfirmPassword = "Password123!",
            Token = "SomeToken"
        };

        // Act
        var result = _validator.TestValidate(dto);
            
    }

    [Fact]
    public void Should_Fail_When_Email_IsEmpty()
    {
        // Arrange
        var dto = new PasswordResetDto
        {
            NewPassword = "Password123!",
            ConfirmPassword = "Password123!",
            Token = "SomeToken"
        };

        // Act
        var result = _validator.TestValidate(dto);

    }

    [Fact]
    public void Should_Fail_When_Password_TooShort()
    {
        // Arrange
        var dto = new PasswordResetDto
        {
            NewPassword = "Short1!", // 7 characters
            ConfirmPassword = "Short1!",
            Token = "SomeToken"
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
            NewPassword = new string('A',
                              101) +
                          "a1!", // 104 characters
            ConfirmPassword = new string('A',
                                  101) +
                              "a1!",
            Token = "SomeToken"
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
            NewPassword = "password123!",
            ConfirmPassword = "password123!",
            Token = "SomeToken"
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
            NewPassword = "PASSWORD123!",
            ConfirmPassword = "PASSWORD123!",
            Token = "SomeToken"
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
            NewPassword = "Password!",
            ConfirmPassword = "Password!",
            Token = "SomeToken"
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
            NewPassword = "Password123",
            ConfirmPassword = "Password123",
            Token = "SomeToken"
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
            NewPassword = "",
            ConfirmPassword = "",
            Token = "SomeToken"
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
            NewPassword = "Password123!",
            ConfirmPassword = "DifferentPassword123!",
            Token = "SomeToken"
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
            NewPassword = "Password123!",
            ConfirmPassword = "",
            Token = "SomeToken"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
    }
}