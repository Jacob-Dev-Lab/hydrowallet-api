using System;
using System.Collections.Generic;
using System.Text;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Tests.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Create_ShouldCreateEmail_WhenValidEmailIsProvided()
        {
            // Arrange
            var validEmail = "test@example.com";

            // Act
            var email = Email.Create(validEmail);

            // Assert
            Assert.NotNull(email);
            Assert.Equal(validEmail, email.Value);
        }

        [Fact]
        public void Create_ShouldTrimAndNormalizeEmail_WhenValidEmailWithWhitespaceIsProvided()
        {
            // Arrange
            var validEmailWithWhitespace = "  test@example.com  ";

            // Act
            var email = Email.Create(validEmailWithWhitespace);

            // Assert
            Assert.NotNull(email);
            Assert.Equal("test@example.com", email.Value);
        }

        [Fact]
        public void Create_ShouldThrow_WhenEmailIsNull()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => Email.Create(null!));
        }

        [Fact]
        public void Create_ShouldThrow_WhenEmailIsEmpty()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => Email.Create(string.Empty));
        }

        [Fact]
        public void Create_ShouldThrow_WhenEmailIsWhitespace()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => Email.Create("   "));
        }

        [Theory]
        [InlineData("invalid-email")]
        [InlineData("test@")]
        [InlineData("@example.com")]
        [InlineData("test@.com")]
        [InlineData("test@@example.com")]
        [InlineData("test.example.com")]
        public void Create_ShouldThrow_WhenEmailFormatIsInvalid(string invalidEmail)
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => Email.Create(invalidEmail));
        }

        [Fact]
        public void Create_ShouldThrow_WhenEmailExceedsMaxLength()
        {
            // Arrange
            var longEmail = new string('a', 256) + "@example.com";

            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => Email.Create(longEmail));
        }

        [Fact]
        public void Create_ShouldThrow_WhenEmailContainsInvalidCharacters()
        {
            // Arrange
            var invalidEmail = "test@exa mple.com";

            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(
                () => Email.Create(invalidEmail));
        }

        [Fact]
        public void ToString_ShouldReturnEmailValue()
        {
            // Arrange
            var validEmail = "test@example.com";

            // Act
            var email = Email.Create(validEmail);

            // Assert
            Assert.NotNull(email);
            Assert.Equal("test@example.com", email.Value);
        }
    }
}
