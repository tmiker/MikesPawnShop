namespace Development.Blazor.DTOs.Write
{
    public class AddProductDTO
    {
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public string Currency { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int QuantityOnHand { get; set; }

        // public int QuantityAllocated { get; set; }

        public string? UOM { get; set; } = default!;
        public int LowStockThreshold { get; set; }
    }
}
