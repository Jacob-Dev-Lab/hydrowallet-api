using System.Text.RegularExpressions;
using Wallet.Domain.Exceptions;

namespace Wallet.Domain.ValueObjects
{
    public sealed partial record MobileNumber
    {
        public string Value { get; }

        [GeneratedRegex(
            @"^\+447\d{9}$",
            RegexOptions.CultureInvariant
        )]
        private static partial Regex MobileNumberRegex();

        private MobileNumber(string value) => Value = value;

        public static MobileNumber Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException(
                    "Mobile number is required");

            var normalized = Normalize(value);

            if (!MobileNumberRegex().IsMatch(normalized))
                throw new DomainValidationException(
                    "Invalid mobile number format. Expected format: +447XXXXXXXXX");

            return new MobileNumber(normalized);
        }

        private static string Normalize(string value)
        {
            var cleaned = value.Trim()
                               .Replace(" ", "")
                               .Replace("-", "");

            if (cleaned.StartsWith("0", StringComparison.Ordinal))
                return "+44" + cleaned[1..];

            if (cleaned.StartsWith("44", StringComparison.Ordinal))
                return "+" + cleaned;

            return cleaned;
        }

        public override string ToString() => Value;
    }
}
