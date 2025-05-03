using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class StablePostUpdateDtoValidatorTests
    {
        private readonly StablePostUpdateDtoValidator _validator;

        public StablePostUpdateDtoValidatorTests()
        {
            _validator = new StablePostUpdateDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_StablePostUpdateDto()
        {
            // Arrange
            var dto = new StablePostUpdateDto
            {
                Id = 1,
                Title = "Updated Announcement",
                Content = "This is an updated announcement for the stable."
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
            var dto = new StablePostUpdateDto
            {
                Id = id,
                Title = "Updated Announcement",
                Content = "This is an updated announcement for the stable."
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Stable post ID must be greater than 0.");
        }

        [Theory]
        [InlineData("", "Title is required.")]
        [InlineData(null, "Title is required.")]
        public void Should_Fail_When_Title_IsNullOrEmpty(string title, string expectedError)
        {
            // Arrange
            var dto = new StablePostUpdateDto
            {
                Id = 1,
                Title = title,
                Content = "This is an updated announcement for the stable."
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_Title_ExceedsMaxLength()
        {
            // Arrange
            var dto = new StablePostUpdateDto
            {
                Id = 1,
                Title = new string('A', 511), // 511 characters, exceeding the 510 character limit
                Content = "This is an updated announcement for the stable."
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Title cannot exceed 510 characters.");
        }

        [Theory]
        [InlineData("", "Content is required.")]
        [InlineData(null, "Content is required.")]
        public void Should_Fail_When_Content_IsNullOrEmpty(string content, string expectedError)
        {
            // Arrange
            var dto = new StablePostUpdateDto
            {
                Id = 1,
                Title = "Updated Announcement",
                Content = content
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage(expectedError);
        }

        [Fact]
        public void Should_Fail_When_Content_ExceedsMaxLength()
        {
            // Arrange
            var dto = new StablePostUpdateDto
            {
                Id = 1,
                Title = "Updated Announcement",
                Content = new string('A', 4095) // 4095 characters, exceeding the 4094 character limit
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage("Content cannot exceed 4094 characters.");
        }
    }
}
