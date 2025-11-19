using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Accounts.API.Domain.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? AccountId { get; set; }
        public string? OwnerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<Address> Addresses { get; set; } = new List<Address>();
        public string? PhoneNumber { get; set; }
        public string? AccountStatus { get; set; }
        public int CreditLimit { get; set; }


        public bool AddAddress(Address address)
        {
            // Note: Equality comparison of this value object does not include properties IsPrimaryBilling and IsPrimaryShipping

            if (address.IsPrimaryBilling)
            {
                Address? primaryBilling = Addresses.FirstOrDefault(a => a.IsPrimaryBilling == true);
                if (primaryBilling != null) primaryBilling.IsPrimaryBilling = false;
            }

            if (address.IsPrimaryShipping)
            {
                Address? primaryShipping = Addresses.FirstOrDefault(a => a.IsPrimaryShipping == true);
                if (primaryShipping != null) primaryShipping.IsPrimaryShipping = false;
            }

            bool exists = false;

            foreach (var item in Addresses)
            {
                if (address.Equals(item))    // or EqualityComparer<Address>.Default.Equals(address, item), or object.Equals(address, item), or address == item (depends on how implement, what override)
                {
                    exists = true;
                    item.IsPrimaryShipping = address.IsPrimaryShipping;
                    item.IsPrimaryBilling = address.IsPrimaryBilling;
                    break;
                }
            }

            if (!exists)
            {
                Addresses.Add(address);
            }

            return true;
        }
    }
}
