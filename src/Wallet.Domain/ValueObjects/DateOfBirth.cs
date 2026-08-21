using System.Globalization;
using Wallet.Domain.Exceptions;

namespace Wallet.Domain.ValueObjects
{
    public sealed record BDateOfBirth
    {
        private const int MinimumAge = 18;
        private const int MaximumAgeYears = 120;
        private const string ExpectedFormat = "yyyy-MM-dd";

        public DateOnly Value { get; }

        private BDateOfBirth(DateOnly value) => Value = value;

        public static BDateOfBirth Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException("Birth date is required");

            if (!DateOnly.TryParseExact(
                    value.Trim(),
                    ExpectedFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var parsedDate))
                throw new DomainValidationException(
                    $"Birth date must be in {ExpectedFormat} format");

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (parsedDate > today)
                throw new DomainValidationException(
                    "Birth date cannot be in the future");

            if (parsedDate < today.AddYears(-MaximumAgeYears))
                throw new DomainValidationException(
                    "Birth date is not valid");

            if (!IsAtLeast(parsedDate, today, MinimumAge))
                throw new DomainValidationException(
                    $"Must be at least {MinimumAge} years old");

            return new BDateOfBirth(parsedDate);
        }

        private static bool IsAtLeast(DateOnly birthDate, DateOnly today, int minimumAge)
        {
            var age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
                age--;

            return age >= minimumAge;
        }

        public override string ToString()
            => Value.ToString(
                ExpectedFormat,
                CultureInfo.InvariantCulture);
    }
}
