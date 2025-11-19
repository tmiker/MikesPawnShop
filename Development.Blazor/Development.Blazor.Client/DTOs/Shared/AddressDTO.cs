namespace Development.Blazor.Client.DTOs.Shared
{
    public class AddressDTO
    {
        public bool IsPrimaryBilling { get; set; }
        public bool IsPrimaryShipping { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        public override string ToString()
        {
            string street = Street2 is null ? $"{Street1}" : $"{Street1}, {Street2}";
            return $"{street}, {City}, {State}, {PostalCode}";
        }
    }
}
