using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Admin.Blazor.Client.DTOs.Products.Write
{
    public class AddImageDTO
    {
        [Required]
        public string ProductId { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Caption { get; set; } = default!;

        // public int SequenceNumber { get; set; }
        [Required]
        public IBrowserFile? ImageBlob { get; set; }
        [Required]
        public string? BlobFileName { get; set; }



        //// PREVIOUS
        //public Guid ProductId { get; init; }
        //public string Name { get; init; } = default!;
        //public string Caption { get; init; } = default!;
        //public int SequenceNumber { get; init; }
        //public string ImageUrl { get; init; } = default!;
        //public string ThumbnailUrl { get; init; } = default!;

        //public AddImageDTO(Guid productId, string name, string caption, int sequenceNumber, string imageUrl, string thumbnailUrl)
        //{
        //    ProductId = productId;
        //    Name = name;
        //    Caption = caption;
        //    SequenceNumber = sequenceNumber;
        //    ImageUrl = imageUrl;
        //    ThumbnailUrl = thumbnailUrl;
        //}
    }
}
