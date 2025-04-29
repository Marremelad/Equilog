using equilog_backend.DTOs.StablePostDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class StablePostCreateDtoValidatorTests
    {
        private readonly StablePostCreateDtoValidator _validator;

        public StablePostCreateDtoValidatorTests()
        {
            _validator = new StablePostCreateDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_StablePostCreateDto()
        {
            // Arrange
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = 5,
                Title = "Important Announcement",
                Content = "Please note that the stable will be closed for maintenance next weekend.",
                Date = DateTime.Now,
                IsPinned = true
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_UserIdFk_NotGreaterThanZero(int userId)
        {
            // Arrange
            var dto = new StablePostCreateDto
            {
                UserIdFk = userId,
                StableIdFk = 5,
                Title = "Important Announcement",
                Content = "Please note that the stable will be closed for maintenance next weekend.",
                Date = DateTime.Now,
                IsPinned = true
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.UserIdFk)
                .WithErrorMessage("User ID must be greater than 0.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Fail_When_StableIdFk_NotGreaterThanZero(int stableId)
        {
            // Arrange
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = stableId,
                Title = "Important Announcement",
                Content = "Please note that the stable will be closed for maintenance next weekend.",
                Date = DateTime.Now,
                IsPinned = true
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StableIdFk)
                .WithErrorMessage("Stable ID must be greater than 0.");
        }

        [Theory]
        [InlineData("", "Title is required.")]
        [InlineData(null, "Title is required.")]
        public void Should_Fail_When_Title_IsNullOrEmpty(string title, string expectedError)
        {
            // Arrange
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = 5,
                Title = title,
                Content = "Please note that the stable will be closed for maintenance next weekend.",
                Date = DateTime.Now,
                IsPinned = true
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
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = 5,
                Title = new string('A', 511), // 511 characters, exceeding the 510 character limit
                Content = "Please note that the stable will be closed for maintenance next weekend.",
                Date = DateTime.Now,
                IsPinned = true
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
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = 5,
                Title = "Important Announcement",
                Content = content,
                Date = DateTime.Now,
                IsPinned = true
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
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = 5,
                Title = "Important Announcement",
                Content = new string('A', 4095), // 4095 characters, exceeding the 4094 character limit
                Date = DateTime.Now,
                IsPinned = true
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Content)
                .WithErrorMessage("Content cannot exceed 4094 characters.");
        }

        [Fact]
        public void Should_Fail_When_Date_IsEmpty()
        {
            // Arrange
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = 5,
                Title = "Important Announcement",
                Content = "Please note that the stable will be closed for maintenance next weekend.",
                Date = default,
                IsPinned = true
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Date)
                .WithErrorMessage("Date is required.");
        }

        [Fact]
        public void Should_Pass_When_IsPinned_IsFalse()
        {
            // Arrange
            var dto = new StablePostCreateDto
            {
                UserIdFk = 1,
                StableIdFk = 5,
                Title = "Regular Announcement",
                Content = "Just a regular update.",
                Date = DateTime.Now,
                IsPinned = false
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.IsPinned);
        }
    }
}
