using System;
using System.Collections.Generic;
using System.Text;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Tests.ValueObjects
{
    public class MobileNumberTests
    {
        [Fact]
        public void Create_ShouldCreateMobileNumber_WhenValidNumberIsProvided()
        {
            // Arrange
            var value = "07123456789";

            // Act
            var mobileNumber = MobileNumber.Create(value);

            // Assert
            Assert.NotNull(mobileNumber);
            Assert.Equal("+447123456789", mobileNumber.Value);
        }

        [Theory]
        [InlineData("07123456789", "+447123456789")]
        [InlineData("+447123456789", "+447123456789")]
        [InlineData("447123456789", "+447123456789")]
        [InlineData("07 123 456 789", "+447123456789")]
        [InlineData("07-123-456-789", "+447123456789")]
        public void Create_ShouldNormalizeMobileNumber(string input, string expected)
        {
            // Act
            var mobileNumber = MobileNumber.Create(input);

            // Assert
            Assert.NotNull(mobileNumber);
            Assert.Equal(expected, mobileNumber.Value);
        }

        [Fact]
        public void Create_ShouldThrow_WhenNumberIsNull()
        {
            // Act & Assert
            Assert.Throws<DomainValidationException>(
                () => MobileNumber.Create(null!));
        }

        [Fact]
        public void Create_ShouldThrow_WhenNumberIsEmpty()
        {
            // Act & Assert
            Assert.Throws<DomainValidationException>(
                () => MobileNumber.Create(""));
        }

        [Fact]
        public void Create_ShouldThrow_WhenNumberIsWhitespace()
        {
            // Act & Assert
            Assert.Throws<DomainValidationException>(
                () => MobileNumber.Create("   "));
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("12345")]
        [InlineData("abcdefghijk")]
        [InlineData("0712345678901234567890")]
        public void Create_ShouldThrow_WhenNumberIsInvalid(string invalidNumber)
        {
            // Act & Assert
            Assert.Throws<DomainValidationException>(
                () => MobileNumber.Create(invalidNumber));
        }

        [Fact]
        public void ToString_ShouldReturnNormalizedValue()
        {
            // Arrange
            var value = "07123456789";
            var mobileNumber = MobileNumber.Create(value);

            // Act
            var result = mobileNumber.ToString();

            // Assert
            Assert.Equal("+447123456789", result);
        }
    }
}
