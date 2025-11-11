using System.Text.Json;

namespace Development.Blazor.DTOs.Tests
{
    public class ProductSnapshotDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityAvailable { get; set; }
        public string? UOM { get; set; }
        public int LowStockThreshold { get; set; }
        public List<ImageDataSnapshotDTO>? Images { get; set; }
        public List<DocumentDataSnapshotDTO>? Documents { get; set; }
        public int Version { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string ToJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            return JsonSerializer.Serialize(this, options);
        }
    }
}
