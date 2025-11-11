
using System.Text.Json.Serialization;

namespace Development.Blazor.DTOs.Read
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public Guid AggregateId { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Currency { get; set; }
        public string? Status { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityAllocated { get; set; }
        public string? UOM { get; set; }
        public int LowStockThreshold { get; set; }
        public int Version { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public List<ImageDataDTO>? Images { get; set; }
        public List<DocumentDataDTO>? Documents { get; set; }

        [JsonIgnore]
        public int QuantityAvailable { get => QuantityOnHand - QuantityAllocated; }
    }
}
