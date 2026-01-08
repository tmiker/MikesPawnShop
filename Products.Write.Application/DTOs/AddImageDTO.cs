using Microsoft.AspNetCore.Http;

namespace Products.Write.Application.DTOs
{
    public class AddImageDTO
    {
        public string ProductId { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Caption { get; init; } = default!;

        // Added for blob service
        public IFormFile? ImageBlob { get; set; }
        public string? BlobFileName { get; set; }
    }
}
