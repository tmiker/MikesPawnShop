using MediatR;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.DTOs;

namespace Products.Write.Application.CQRS.Commands
{
    public class DeleteDocument : IRequest<DeleteDocumentResult>
    {
        public Guid ProductId { get; init; }
        public string FileName { get; init; } = default!;
        public string? CorrelationId { get; set; }

        public DeleteDocument(DeleteDocumentDTO deleteDocumentDTO, string? correlationId)
        {
            ProductId = Guid.Parse(deleteDocumentDTO.ProductId);
            FileName = deleteDocumentDTO.FileName;
            CorrelationId = correlationId;
        }
    }
}
