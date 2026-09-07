using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Tests.ValueObjects
{
    public class DateOfBirthTests
    {
        [Fact]
        public void Create_ShouldCreateDateOfBirth_WhenValidDateIsProvided()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var birthDate = today.AddYears(-30);

            // Act
            var dateOfBirth = DateOfBirth
                .Create(birthDate.ToString("yyyy-MM-dd"));

            // Assert
            Assert.NotNull(dateOfBirth);
            Assert.Equal(birthDate, dateOfBirth.Value);
        }

        [Fact]
        public void Create_ShouldThrow_WhenDateIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => DateOfBirth.Create(null!));
        }

        [Fact]
        public void Create_ShouldThrow_WhenDateIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => DateOfBirth.Create(string.Empty));
        }

        [Fact]
        public void Create_ShouldThrow_WhenDaateIsWhitespace()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => DateOfBirth.Create("   "));
        }

        [Fact]
        public void Create_ShouldThrow_WhenDateIsInFuture()
        {
            // Arrange
            var futureDate = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(1);

            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => DateOfBirth.Create(futureDate.ToString("yyyy-MM-dd")));
        }

        [Theory]
        [InlineData("invalid-date")]
        [InlineData("2000-13-45")]
        [InlineData("2002-02-30")]
        [InlineData("10-10-2000")]
        [InlineData("2000/10/10")]
        [InlineData("10/10/2000")]
        public void Create_ShouldThrow_WhenDateIsInvalid(string invalidDate)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => DateOfBirth.Create(invalidDate));
        }

        [Fact]
        public void Create_ShouldThrow_WhenAgeIsLessThan18()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var birthDate = today.AddYears(-17);

            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => DateOfBirth.Create(birthDate.ToString("yyyy-MM-dd")));
        }

        [Fact]
        public void Create_ShouldAllowWhenAgeIsExactly18()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var birthDate = today.AddYears(-18);

            // Act
            var dateOfBirth = DateOfBirth.Create(birthDate.ToString("yyyy-MM-dd"));

            // Assert
            Assert.NotNull(dateOfBirth);
            Assert.Equal(birthDate, dateOfBirth.Value);
        }

        [Fact]
        public void Create_ShouldThrow_WhenAgeIsGreaterThan120()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var birthDate = today.AddYears(-121);

            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => DateOfBirth.Create(birthDate.ToString("yyyy-MM-dd")));
        }

        [Fact]
        public void ToString_ShouldReturnDateInExpectedFormat()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var birthDate = today.AddYears(-30);
            var dateOfBirth = DateOfBirth.Create(birthDate.ToString("yyyy-MM-dd"));

            // Act
            var result = dateOfBirth.ToString();

            // Assert
            Assert.Equal(birthDate.ToString("yyyy-MM-dd"), result);
        }

        [Fact]
        public void Create_ShouldTrimWhitespaceFromInput()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var birthDate = today.AddYears(-30);
            var input = $"  {birthDate.ToString("yyyy-MM-dd")}  ";

            // Act
            var dateOfBirth = DateOfBirth.Create(input);

            // Assert
            Assert.NotNull(dateOfBirth);
            Assert.Equal(birthDate, dateOfBirth.Value);
        }
    }
}
