using System.Text.RegularExpressions;

namespace Products.Write.Domain.ValueObjects.Alt
{
    public record Email(string Value)
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("Invalid email format", nameof(email));

            return new Email(email.ToLowerInvariant());
        }

        public static implicit operator string(Email email) => email.Value;
    }
}
