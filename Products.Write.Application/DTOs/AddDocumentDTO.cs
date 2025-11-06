using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.DTOs
{
    public class AddDocumentDTO
    {
        [Required]
        public string ProductId { get; init; } = default!;
        [Required]
        public string Name { get; init; } = default!;
        [Required]
        public string Title { get; init; } = default!;
        [Required]
        public int SequenceNumber { get; init; }

        // public string? DocumentUrl { get; init; } = default!;

        // Added for blob service
        [Required]
        public IFormFile? DocumentBlob { get; set; }
        [Required]
        public string? BlobFileName { get; set; }

        //public AddDocumentDTO(Guid productId, string name, string title, int sequenceNumber)
        //{
        //    ProductId = productId;
        //    Name = name;
        //    Title = title;
        //    SequenceNumber = sequenceNumber;
        //    // DocumentUrl = documentUrl;
        //}
    }
}
