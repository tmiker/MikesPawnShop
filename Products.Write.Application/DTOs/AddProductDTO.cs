namespace Products.Write.Application.DTOs
{
    public class AddProductDTO
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

        public AddProductDTO(string name, string category, string description, 
            decimal price, string currency, string status, int quantityOnHand,
            int quantityAvailable, string uom, int lowStockThreshold)
        {
            Name = name;
            Category = category;    
            Description = description;
            Price = price;
            Currency = currency;
            Status = status;       
            QuantityOnHand = quantityOnHand;
            QuantityAvailable = quantityAvailable;
            UOM = uom;
            LowStockThreshold = lowStockThreshold;
        }
    }
}
