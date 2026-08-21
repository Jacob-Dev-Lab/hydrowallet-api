using System.Text.RegularExpressions;
using Wallet.Domain.Exceptions;

namespace Wallet.Domain.ValueObjects
{
    public sealed partial record Email
    {
        public const int MaxLength = 100;

        public string Value { get; }

        [GeneratedRegex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.CultureInvariant
        )]
        private static partial Regex EmailRegex();

        private Email(string value) => Value = value;

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException(
                    "Email address is required");

            var normalized = value.Trim().ToLowerInvariant();

            if (normalized.Length > MaxLength)
                throw new DomainValidationException(
                    $"Email address cannot exceed {MaxLength} characters");

            if (!EmailRegex().IsMatch(normalized))
                throw new DomainValidationException(
                    "Invalid email address format");

            return new Email(normalized);
        }

        public override string ToString() => Value;
    }
}
