using Microsoft.AspNetCore.Http;

namespace Products.Write.Application.DTOs
{
    public class AddDocumentDTO
    {
        public string? ProductId { get; init; } 
        public string? Name { get; init; } 
        public string? Title { get; init; }
        // for blob service
        public IFormFile? DocumentBlob { get; set; } 
        public string? BlobFileName { get; set; } 
    }
}
