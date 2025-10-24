namespace Products.Write.Domain.ValueObjects.Alt
{
    public record Address(
        string Street,
        string City,
        string State,
        string PostalCode,
        string Country)
    {
        public static Address Create(string street, string city, string state, string postalCode, string country)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty", nameof(street));

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty", nameof(city));

            if (string.IsNullOrWhiteSpace(postalCode))
                throw new ArgumentException("Postal code cannot be empty", nameof(postalCode));

            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be empty", nameof(country));

            return new Address(street, city, state, postalCode, country);
        }

        public string GetFullAddress()
        {
            return $"{Street}, {City}, {State} {PostalCode}, {Country}";
        }
    }
}
