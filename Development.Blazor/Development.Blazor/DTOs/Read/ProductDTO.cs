using System.ComponentModel.DataAnnotations.Schema;

namespace Development.Blazor.DTOs.Read
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public Guid AggregateId { get; private set; }
        public string? Name { get; private set; }
        public string? Category { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public string? Currency { get; private set; }
        public string? Status { get; private set; }
        public List<ImageDataDTO>? Images { get; set; }
        public List<DocumentDataDTO>? Documents { get; set; }
        public int Version { get; set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateUpdated { get; private set; }
    }
}
