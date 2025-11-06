using Products.Write.Application.DTOs;

namespace Products.Write.Application.CQRS.Commands
{
    public class DeleteImage
    {
        public Guid ProductId { get; init; }
        public string FileName { get; init; } = default!;
        public string? CorrelationId { get; set; }

        public DeleteImage(DeleteImageDTO deleteImageDTO, string? correlationId)
        {
            ProductId = Guid.Parse(deleteImageDTO.ProductId);
            FileName = deleteImageDTO.FileName;
            CorrelationId = correlationId;
        }
    }
}
