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
        public List<ImageData>? Images { get; set; } 

        [InverseProperty(nameof(DocumentData.Product))]
        public List<DocumentData>? Documents { get; set; } 

        public int Version { get; set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateUpdated { get; set; }

        private Product() { }

        public Product(Guid aggregateId, string? name, string? category, string? description, decimal price, string? currency, string? status, int version)
        {
            AggregateId = aggregateId;
            Name = name;
            Category = category;
            Description = description;
            Price = price;
            Currency = currency;
            Status = status;
            Version = version;
            DateCreated = DateTime.UtcNow;
            DateUpdated = default;
        }

        public void AddImage(ImageData image, int version)
        {
            if (Images is null) Images = new List<ImageData>();
            Images.Add(image);
            Version = version;
            DateUpdated = DateTime.UtcNow;
        }

        public void AddDocument(DocumentData document, int version)
        {
            if (Documents is null) Documents = new List<DocumentData>();
            Documents.Add(document);
            Version = version;
            DateUpdated = DateTime.UtcNow;
        }

        public void UpdateStatus(string status, int version)
        {
            Status = status;
            Version = version;
            DateUpdated = DateTime.UtcNow;
        }
    }
}
