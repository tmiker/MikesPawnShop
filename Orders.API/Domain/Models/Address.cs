using Orders.API.DTOs;

namespace Orders.API.Domain.Models
{
    public class Address
    {
        public bool IsPrimaryBilling { get; set; }
        public bool IsPrimaryShipping { get; set; }
        public string? Street1 { get; set; }
        public string? Street2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        private Address() { }

        public Address(AddressDTO dto)
        {
            IsPrimaryBilling = dto.IsPrimaryBilling;
            IsPrimaryShipping = dto.IsPrimaryShipping;
            Street1 = dto.Street1;
            Street2 = dto.Street2;
            City = dto.City;
            State = dto.State;
            PostalCode = dto.PostalCode;
        }

        public AddressDTO ToDTO()
        {
            return new AddressDTO
            {
                IsPrimaryBilling = IsPrimaryBilling,
                IsPrimaryShipping = IsPrimaryShipping,
                Street1 = Street1,
                Street2 = Street2,
                City = City,
                State = State,
                PostalCode = PostalCode
            };
        }
    }
}
