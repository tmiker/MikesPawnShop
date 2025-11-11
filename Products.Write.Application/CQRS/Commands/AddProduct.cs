using Microsoft.Extensions.Primitives;
using Products.Write.Application.DTOs;
using System.Text;

namespace Products.Write.Application.CQRS.Commands
{
    public class AddProduct
    {
        public string Name { get; init; } = default!;
        public string Category { get; init; }
        public string Description { get; init; } = default!;
        public decimal Price { get; init; }
        public string Currency { get; init; } = default!;
        public string Status { get; init; } = default!;
        public int QuantityOnHand { get; init; }
        public int QuantityAvailable { get; init; }
        public string UOM { get; init; } = default!;
        public int LowStockThreshold { get; init; }
        public string? CorrelationId { get; set; } // = default!;

        public AddProduct(string name, string category, string description, decimal price, string currency, string status,
            int quantityOnHand, int quantityAvailable, string uom, int lowStockThreshold, string? correlationId)
        {
            Name = name;
            Category = category;    // (CategoryEnum)Enum.Parse(typeof(CategoryEnum), category);
            Description = description;
            Price = price;
            Currency = currency;
            Status = status;        // Status.FromName(status);
            QuantityOnHand = quantityOnHand;
            QuantityAvailable = quantityAvailable;
            UOM = uom;
            LowStockThreshold = lowStockThreshold;
            CorrelationId = correlationId;
        }

        public AddProduct(AddProductDTO dto, StringValues correlationId)
        {
            Name = dto.Name;
            Category = dto.Category;        
            Description = dto.Description;
            Price = dto.Price;
            Currency = dto.Currency;
            Status = dto.Status;            
            QuantityOnHand = dto.QuantityOnHand;
            QuantityAvailable = dto.QuantityAvailable;
            UOM = dto.UOM;
            LowStockThreshold = dto.LowStockThreshold;
            CorrelationId = correlationId!;
        }
    }
}
