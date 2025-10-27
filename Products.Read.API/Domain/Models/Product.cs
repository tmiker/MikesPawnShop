using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Read.API.Domain.Models
{
    [Table("Products", Schema = "dbo")]
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid AggregateId { get; private set; }
        public string? Name { get; private set; }
        public string? Category { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public string? Currency { get; private set; }
        public string? Status { get; private set; }

        [InverseProperty(nameof(ImageData.Product))]
        public List<ImageData>? Images { get; private set; } = new List<ImageData>();

        [InverseProperty(nameof(DocumentData.Product))]
        public List<DocumentData>? Documents { get; private set; } = new List<DocumentData>();

        public int Version { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateUpdated { get; private set; }

        private Product() { }

        public Product(Guid aggregateId, string? name, string? category, string? description, decimal price, string? currency, string? status, List<ImageData>? images, List<DocumentData>? documents, int version, DateTime dateCreated, DateTime dateUpdated)
        {
            AggregateId = aggregateId;
            Name = name;
            Category = category;
            Description = description;
            Price = price;
            Currency = currency;
            Status = status;
            Images = images;
            Documents = documents;
            Version = version;
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
        }
    }
}
