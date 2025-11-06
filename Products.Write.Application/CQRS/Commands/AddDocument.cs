using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Products.Write.Application.DTOs;

namespace Products.Write.Application.CQRS.Commands
{
    public class AddDocument
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = default!;          // for Azure blob storage, virtual directory plus filename
        public string Title { get; init; } = default!;
        public int SequenceNumber { get; init; }
        public string? CorrelationId { get; set; }
        public IFormFile? DocumentBlob { get; set; }
        public string? BlobFileName { get; set; }

        public AddDocument(AddDocumentDTO addDocumentDTO, StringValues correlationId)
        {
            ProductId = Guid.Parse(addDocumentDTO.ProductId);
            Name = addDocumentDTO.Name;
            Title = addDocumentDTO.Title;
            SequenceNumber = addDocumentDTO.SequenceNumber;
            CorrelationId = correlationId;
            DocumentBlob = addDocumentDTO.DocumentBlob;
            BlobFileName = addDocumentDTO.BlobFileName;
        }
    }
}
