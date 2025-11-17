using Orders.API.DTOs;

namespace Orders.API.Domain.Models
{
    public class OrderItem
    {
        public string? OrderId { get; set; }
        public int LineNumber { get; set; }
        public int ProductId { get; set; }
        public string? AggregateId { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Currency { get; set; }
        public decimal Price { get; set; }
        public double Quantity { get; set; }

        public OrderItem(AddOrderItemDTO dto)
        {
            ProductId = dto.ProductId;
            AggregateId = dto.AggregateId;
            Category = dto.Category;
            Name = dto.Name;
            Currency = dto.Currency;
            Price = dto.Price;
            Quantity = dto.Quantity;
        }

        public OrderItem(OrderItemDTO dto)
        {
            OrderId = dto.OrderId;
            LineNumber = dto.LineNumber;
            ProductId = dto.ProductId;
            AggregateId = dto.AggregateId;
            Category = dto.Category;
            Name = dto.Name;
            Currency = dto.Currency;
            Price = dto.Price;
            Quantity = dto.Quantity;
        }

        public OrderItemDTO ToDTO()
        {
            return new OrderItemDTO()
            {
                OrderId = this.OrderId,
                LineNumber = this.LineNumber,
                ProductId = this.ProductId,
                AggregateId = this.AggregateId,
                Category = this.Category,
                Name = this.Name,
                Currency = this.Currency,
                Price = this.Price,
                Quantity = this.Quantity
            };
        }
    }
}
