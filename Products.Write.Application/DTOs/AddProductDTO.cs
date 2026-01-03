using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.DTOs
{
    public class AddProductDTO
    {
        [Required]
        public string Name { get; init; } = default!;
        [Required]
        public string Category { get; init; }
        [Required]
        public string Description { get; init; } = default!;
        [Required]
        public decimal Price { get; init; }
        [Required]
        public string Currency { get; init; } = default!;
        [Required]
        public string Status { get; init; } = default!;
        [Required]
        public int QuantityOnHand { get; init; }
        [Required]
        public string UOM { get; init; } = default!;
        [Required]
        public int LowStockThreshold { get; init; }

        public AddProductDTO(string name, string category, string description, 
            decimal price, string currency, string status, int quantityOnHand,
            string uom, int lowStockThreshold)
        {
            Name = name;
            Category = category;    
            Description = description;
            Price = price;
            Currency = currency;
            Status = status;       
            QuantityOnHand = quantityOnHand;
            UOM = uom;
            LowStockThreshold = lowStockThreshold;
        }
    }
}
