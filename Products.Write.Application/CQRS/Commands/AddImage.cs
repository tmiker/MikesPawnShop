using Microsoft.Extensions.Primitives;
using Products.Write.Application.DTOs;

namespace Products.Write.Application.CQRS.Commands
{
    public class AddImage
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = default!;       // virtual directory plus filename
        public string Caption { get; init; } = default!;
        public int SequenceNumber { get; init; }
        public string ImageUrl { get; init; } = default!;
        public string ThumbnailUrl { get; init; } = default!;
        public string? CorrelationId { get; set; } 

        public AddImage(AddImageDTO addImageDTO, StringValues correlationId)
        {
            ProductId = addImageDTO.ProductId;
            Name = addImageDTO.Name;
            Caption = addImageDTO.Caption;
            SequenceNumber = addImageDTO.SequenceNumber;
            ImageUrl = addImageDTO.ImageUrl;
            ThumbnailUrl = addImageDTO.ThumbnailUrl;
            CorrelationId = correlationId;
        }
    }
}
