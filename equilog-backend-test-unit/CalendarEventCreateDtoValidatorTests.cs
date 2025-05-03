using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class CalendarEventCreateDtoValidatorTests
    {
        private readonly CalendarEventCreateDtoValidator _validator;

        public CalendarEventCreateDtoValidatorTests()
        {
            _validator = new CalendarEventCreateDtoValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_CalendarEventCreateDto()
        {
            // Arrange
            var dto = new CalendarEventCreateDto
            {
                Title = "Horse Riding Lesson",
                StartDateTime = DateTime.Now.AddHours(1),
                EndDateTime = DateTime.Now.AddHours(2),
                UserIdFk = 1,
                StableIdFk = 5
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "Title is required.")]
        [InlineData(null, "Title is required.")]
        public void Should_Fail_When_Title_IsNullOrEmpty(string title, string expectedError)
        {
            // Arrange
            var dto = new CalendarEventCreateDto
            {
                Title = title,
                StartDateTime = DateTime.Now.AddHours(1),
                EndDateTime = DateTime.Now.AddHours(2),
                UserIdFk = 1,
                StableIdFk = 5
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
            var dto = new CalendarEventCreateDto
            {
                Title = new string('A', 51), // 51 characters, exceeding the 50 character limit
                StartDateTime = DateTime.Now.AddHours(1),
                EndDateTime = DateTime.Now.AddHours(2),
                UserIdFk = 1,
                StableIdFk = 5
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Title)
                .WithErrorMessage("Title cannot exceed 50 characters.");
        }

        [Fact]
        public void Should_Fail_When_StartDateTime_IsEmpty()
        {
            // Arrange
            var dto = new CalendarEventCreateDto
            {
                Title = "Horse Riding Lesson",
                StartDateTime = default,
                EndDateTime = DateTime.Now.AddHours(2),
                UserIdFk = 1,
                StableIdFk = 5
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StartDateTime)
                .WithErrorMessage("Start is required.");
        }

        [Fact]
        public void Should_Fail_When_EndDateTime_IsEmpty()
        {
            // Arrange
            var dto = new CalendarEventCreateDto
            {
                Title = "Horse Riding Lesson",
                StartDateTime = DateTime.Now.AddHours(1),
                EndDateTime = default,
                UserIdFk = 1,
                StableIdFk = 5
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EndDateTime)
                .WithErrorMessage("End is required.");
        }

        [Fact]
        public void Should_Fail_When_StableIdFk_IsEmpty()
        {
            // Arrange
            var dto = new CalendarEventCreateDto
            {
                Title = "Horse Riding Lesson",
                StartDateTime = DateTime.Now.AddHours(1),
                EndDateTime = DateTime.Now.AddHours(2),
                UserIdFk = 1,
                StableIdFk = 0 // Default value for int, considered empty
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StableIdFk)
                .WithErrorMessage("Stable ID must be greater than 0.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Fail_When_StableIdFk_NotGreaterThanZero(int stableId)
        {
            // Arrange
            var dto = new CalendarEventCreateDto
            {
                Title = "Horse Riding Lesson",
                StartDateTime = DateTime.Now.AddHours(1),
                EndDateTime = DateTime.Now.AddHours(2),
                UserIdFk = 1,
                StableIdFk = stableId
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.StableIdFk)
                .WithErrorMessage("Stable ID must be greater than 0.");
        }
    }
}
