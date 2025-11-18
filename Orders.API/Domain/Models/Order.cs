using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Orders.API.DTOs;

namespace Orders.API.Domain.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string OrderId { get; set; }
        public string OwnerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public string Status { get; set; }
        public int Version { get; set; }
        public Address? ShippingAddress { get; set; }
        public Address? BillingAddress { get; set; }

        public Order(string ownerId, List<OrderItem> items, Address? shippingAddress, Address? billingAddress)
        {
            int lineNumber = 1;
            OrderId = Guid.NewGuid().ToString();
            OwnerId = ownerId;
            OrderDate = DateTime.Now;
            Items = items;
            items.ForEach(i => i.LineNumber = lineNumber++);
            items.ForEach(i => i.OrderId = OrderId);
            Status = "Placed";
            Version = 0;
            ShippingAddress = shippingAddress;
            BillingAddress = billingAddress;
        }

        public OrderDTO ToDTO()
        {
            return new OrderDTO
            {
                Id = this.Id,
                OrderId = this.OrderId,
                OrderDate = this.OrderDate,
                Items = this.Items.Select(i => i.ToDTO()).ToList(),
                Status = this.Status,
                Version = this.Version,
                ShippingAddress = this.ShippingAddress?.ToDTO(),
                BillingAddress = this.BillingAddress?.ToDTO()
            };
        }
    }
}
