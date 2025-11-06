using Microsoft.AspNetCore.Http;
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
        public string? CorrelationId { get; set; }
        public IFormFile? ImageBlob { get; set; }
        public string? BlobFileName { get; set; }

        public AddImage(AddImageDTO addImageDTO, StringValues correlationId)
        {
            ProductId = Guid.Parse(addImageDTO.ProductId);
            Name = addImageDTO.Name;
            Caption = addImageDTO.Caption;
            SequenceNumber = addImageDTO.SequenceNumber;
            CorrelationId = correlationId;
            ImageBlob = addImageDTO.ImageBlob;
            BlobFileName = addImageDTO.BlobFileName;
        }
    }
}
