using Microsoft.AspNetCore.Http;

namespace Products.Write.Application.DTOs
{
    public class AddDocumentDTO
    {
        public string ProductId { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Title { get; init; } = default!;
        // for blob service
        public IFormFile? DocumentBlob { get; set; }
        public string? BlobFileName { get; set; }
    }
}
