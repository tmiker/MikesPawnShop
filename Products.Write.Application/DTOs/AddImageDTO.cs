using Microsoft.AspNetCore.Http;

namespace Products.Write.Application.DTOs
{
    public class AddImageDTO
    {
        public string? ProductId { get; init; } 
        public string? Name { get; init; } 
        public string? Caption { get; init; } 
        // Added for blob service
        public IFormFile? ImageBlob { get; set; } 
        public string? BlobFileName { get; set; } 
    }
}
