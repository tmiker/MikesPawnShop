namespace Products.Write.Domain.ValueObjects.Alt
{
    public record Money(decimal Amount, string Currency)
    {
        public static Money Zero(string currency) => new(0, currency);
        public static Money USD(decimal amount) => new(amount, "USD");
        public static Money EUR(decimal amount) => new(amount, "EUR");

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot add different currencies: {Currency} and {other.Currency}");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot subtract different currencies: {Currency} and {other.Currency}");

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Multiply(decimal factor)
        {
            return new Money(Amount * factor, Currency);
        }

        public bool IsGreaterThan(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException($"Cannot compare different currencies: {Currency} and {other.Currency}");

            return Amount > other.Amount;
        }

        public bool IsZeroOrNegative() => Amount <= 0;
    }
}
