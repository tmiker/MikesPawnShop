using System.Text;

namespace Consumer.Blazor.Client.DTOs.Accounts
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
            StringBuilder sb = new StringBuilder();
            sb.Append(Street1);
            if (!string.IsNullOrWhiteSpace(Street2)) sb.Append($", {Street2}");
            sb.Append($", {City}, {State} {PostalCode}");
            return sb.ToString();
        }
    }
}

    
