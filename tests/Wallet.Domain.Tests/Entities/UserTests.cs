using System;
using System.Collections.Generic;
using System.Text;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Tests.Entities
{
    public class UserTests
    {
        [Fact]
        public void Create_ShouldCreatePendingUser()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            // Act
            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserStatus.Pending, user.UserStatus);
        }

        [Fact]
        public void Create_ShouldSetRegisteredAt()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            // Act
            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Assert
            Assert.NotNull(user);
            Assert.True(user.RegisteredAt <= DateTimeOffset.UtcNow);
        }

        [Fact]
        public void Create_ShouldSetId()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            // Act
            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Assert
            Assert.NotNull(user);
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Fact]
        public void ShouldThrow_WhenFirstNameIsEmpty()
        {
            // Arrange
            string firstName = "";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash));

        }

        [Fact]
        public void ShouldThrow_WhenOtherNamesIsEmpty()
        {
            // Arrange
            var firstName = "John";
            string othernames = "";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash));
        }

        [Fact]
        public void ShouldThrow_WhenPasswordHashIsEmpty()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            string passwordHash = "";

            // Act & Assert
            Assert.Throws<DomainValidationException>(() => User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash));
        }

        [Fact]
        public void Activate_ShouldChangePendingUserToActive()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Act
            user.Activate();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserStatus.Active, user.UserStatus);
        }

        [Fact]
        public void Activate_ShouldThrow_WhenUserIsAlreadyActive()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            user.Activate();

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(
                () => user.Activate());
        }

        [Fact]
        public void Activate_ShouldThrow_WhenUserIsDeleted()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            user.Delete();

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(
                () => user.Activate());
        }

        [Fact]
        public void Lock_ShouldLockActiveUser()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            user.Activate();

            // Act
            user.Lock();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserStatus.Locked, user.UserStatus);
        }

        [Fact]
        public void Lock_ShouldThrow_WhenUserIsNotActive()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(
                () => user.Lock());
        }

        [Fact]
        public void Unlock_ShouldUnlockLockedUser()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            user.Activate();
            user.Lock();

            // Act
            user.Unlock();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserStatus.Active, user.UserStatus);
        }

        [Fact]
        public void Unlock_ShouldThrow_WhenUserIsNotLocked()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(
                () => user.Unlock());
        }

        [Fact]
        public void Suspend_ShouldSuspendActiveUser()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Act
            user.Activate();
            user.Suspend();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserStatus.Suspended, user.UserStatus);
        }

        [Fact]
        public void Reinstate_ShouldReinstateSuspendedUser()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            user.Activate();
            user.Suspend();

            // Act
            user.Reinstate();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserStatus.Active, user.UserStatus);
        }

        [Fact]
        public void Reinstate_ShouldThrow_WhenUserIsNotSuspended()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Act & Assert
            Assert.Throws<BusinessRuleViolationException>(
                () => user.Reinstate());
        }

        [Fact]
        public void Delete_ShouldDeleteUser()
        {
            // Arrange
            var firstName = "John";
            var othernames = "Doe";
            var dateOfBirth = DateOfBirth.Create("2000-01-01");
            var email = Email.Create("john.doe@example.com");
            var mobileNumber = MobileNumber.Create("07123456789");
            var passwordHash = "hashed_password";

            var user = User.Create(
                firstName,
                othernames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);

            // Act
            user.Delete();

            // Assert
            Assert.NotNull(user);
            Assert.Equal(UserStatus.Deleted, user.UserStatus);
        }
    }
}
