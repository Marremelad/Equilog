using equilog_backend.DTOs.CalendarEventDTOs;
using equilog_backend.Validators;
using FluentValidation.TestHelper;

namespace equilog_backend_test_unit
{
    public class CalendarEventUpdateValidatorTests
    {
        private readonly CalendarEventUpdateValidator _validator;

        public CalendarEventUpdateValidatorTests()
        {
            _validator = new CalendarEventUpdateValidator();
        }

        [Fact]
        public void Should_Pass_When_Valid_CalendarEventUpdateDto()
        {
            // Arrange
            var dto = new CalendarEventUpdateDto
            {
                Id = 1,
                Title = "Updated Horse Riding Lesson",
                StartDateTime = DateTime.Now.AddHours(3),
                EndDateTime = DateTime.Now.AddHours(4)
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
            var dto = new CalendarEventUpdateDto
            {
                Id = id,
                Title = "Updated Horse Riding Lesson",
                StartDateTime = DateTime.Now.AddHours(3),
                EndDateTime = DateTime.Now.AddHours(4)
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Calendar ID must be greater than 0.");
        }

        [Theory]
        [InlineData("", "Title is required.")]
        [InlineData(null, "Title is required.")]
        public void Should_Fail_When_Title_IsNullOrEmpty(string title, string expectedError)
        {
            // Arrange
            var dto = new CalendarEventUpdateDto
            {
                Id = 1,
                Title = title,
                StartDateTime = DateTime.Now.AddHours(3),
                EndDateTime = DateTime.Now.AddHours(4)
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
            var dto = new CalendarEventUpdateDto
            {
                Id = 1,
                Title = new string('A', 51), // 51 characters, exceeding the 50 character limit
                StartDateTime = DateTime.Now.AddHours(3),
                EndDateTime = DateTime.Now.AddHours(4)
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
            var dto = new CalendarEventUpdateDto
            {
                Id = 1,
                Title = "Updated Horse Riding Lesson",
                StartDateTime = default,
                EndDateTime = DateTime.Now.AddHours(4)
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
            var dto = new CalendarEventUpdateDto
            {
                Id = 1,
                Title = "Updated Horse Riding Lesson",
                StartDateTime = DateTime.Now.AddHours(3),
                EndDateTime = default
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.EndDateTime)
                .WithErrorMessage("End is required.");
        }
    }
}
