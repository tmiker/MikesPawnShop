using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Read.API.Domain.Models
{
    [Table("DocumentData", Schema = "dbo")]
    public class DocumentData
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Name { get; private set; }           
        public string? Title { get; private set; }
        public int SequenceNumber { get; private set; }
        public string? DocumentUrl { get; private set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        [InverseProperty(nameof(Product.Documents))]
        public Product? Product { get; set; } = default!;

        private DocumentData() { }

        public DocumentData(string name, string title, int sequenceNumber, string documentUrl)
        {
            Name = name;
            Title = title;
            SequenceNumber = sequenceNumber;
            DocumentUrl = documentUrl;
        }

        private void SetSequenceNumber(int sequenceNumber)
        {
            SequenceNumber = sequenceNumber;
        }
    }
}
