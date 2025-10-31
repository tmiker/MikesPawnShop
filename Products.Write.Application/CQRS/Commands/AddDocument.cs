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
        public string DocumentUrl { get; init; } = default!;
        public string? CorrelationId { get; set; } 

        public AddDocument(AddDocumentDTO addDocumentDTO, StringValues correlationId)
        {
            ProductId = addDocumentDTO.ProductId;
            Name = addDocumentDTO.Name;
            Title = addDocumentDTO.Title;
            SequenceNumber = addDocumentDTO.SequenceNumber;
            DocumentUrl = addDocumentDTO.DocumentUrl;
            CorrelationId = correlationId;
        }
    }
}
