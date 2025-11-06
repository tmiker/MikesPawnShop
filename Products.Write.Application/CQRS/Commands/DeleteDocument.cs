using Products.Write.Application.DTOs;

namespace Products.Write.Application.CQRS.Commands
{
    public class DeleteDocument
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
