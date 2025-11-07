using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.DTOs
{
    public class AddImageDTO
    {
        [Required]
        public string ProductId { get; init; } = default!;
        [Required]
        public string Name { get; init; } = default!;
        [Required]
        public string Caption { get; init; } = default!;

        //[Required]
        //public int SequenceNumber { get; init; }  // moved to domain

        //public string? ImageUrl { get; init; } = default!;

        //public string? ThumbnailUrl { get; init; } = default!;

        // Added for blob service
        [Required]
        public IFormFile? ImageBlob { get; set; }
        [Required]
        public string? BlobFileName { get; set; }
    }
}
