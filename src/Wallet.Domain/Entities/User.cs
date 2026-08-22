using Wallet.Domain.Enums;
using Wallet.Domain.Exceptions;
using Wallet.Domain.ValueObjects;

namespace Wallet.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }

        public string FirstName { get; private set; } = string.Empty;
        public string OtherNames { get; private set; } = string.Empty;

        public DateOfBirth DateOfBirth { get; private set; }
        public Email Email { get; private set; }
        public MobileNumber MobileNumber { get; private set; }

        public string PasswordHash { get; private set; } = string.Empty;

        public UserStatus UserStatus { get; private set; }
        public DateTimeOffset RegisteredAt { get; private set; }

        private User(
            string firstName,
            string otherNames,
            DateOfBirth dateOfBirth,
            Email email,
            MobileNumber mobileNumber,
            string passwordHash)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            OtherNames = otherNames;
            DateOfBirth = dateOfBirth;
            Email = email;
            MobileNumber = mobileNumber;
            PasswordHash = passwordHash;
            UserStatus = UserStatus.Pending;
            RegisteredAt = DateTimeOffset.UtcNow;
        }

        public static User Create(
            string firstName,
            string otherNames,
            DateOfBirth dateOfBirth,
            Email email,
            MobileNumber mobileNumber,
            string passwordHash)
        {
            ValidateEntries(firstName, otherNames, passwordHash);

            return new User(
                firstName,
                otherNames,
                dateOfBirth,
                email,
                mobileNumber,
                passwordHash);
        }

        public void Activate()
        {
            if (UserStatus == UserStatus.Active)
                throw new BusinessRuleViolationException(
                    "User is already active");

            if (UserStatus == UserStatus.Deleted)
                throw new BusinessRuleViolationException(
                    "A deleted user cannot be activated");

            UserStatus = UserStatus.Active;
        }

        public void Lock()
        {
            if (UserStatus == UserStatus.Locked)
                throw new BusinessRuleViolationException(
                    "User is already locked");

            if (UserStatus != UserStatus.Active)
                throw new BusinessRuleViolationException(
                    "Only an active user can be locked");

            UserStatus = UserStatus.Locked;
        }

        public void Unlock()
        {
            if (UserStatus != UserStatus.Locked)
                throw new BusinessRuleViolationException(
                    "User is not locked");

            UserStatus = UserStatus.Active;
        }

        public void Suspend()
        {
            if (UserStatus == UserStatus.Suspended)
                throw new BusinessRuleViolationException(
                    "User is already suspended");

            if (UserStatus != UserStatus.Active)
                throw new BusinessRuleViolationException(
                    "Only an active user can be suspended");

            UserStatus = UserStatus.Suspended;
        }

        public void Reinstate()
        {
            if (UserStatus != UserStatus.Suspended)
                throw new BusinessRuleViolationException(
                    "User is not suspended");

            UserStatus = UserStatus.Active;
        }

        public void Delete()
        {
            if (UserStatus == UserStatus.Deleted)
                throw new BusinessRuleViolationException("User is already deleted");

            UserStatus = UserStatus.Deleted;
        }

        private static void ValidateEntries(string firstName, string otherNames, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainValidationException("First name is required");

            if (string.IsNullOrWhiteSpace(otherNames))
                throw new DomainValidationException("Other names are required");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainValidationException("Password is required");
        }
    }
}
