namespace Products.Write.Application.DTOs
{
    public class DeleteImageDTO
    {
        public string ProductId { get; init; }
        public string FileName { get; init; } = default!;
        public string? CorrelationId { get; set; }

        public DeleteImageDTO(string productId, string fileName, string? correlationId)
        {
            ProductId = productId;
            FileName = fileName;
            CorrelationId = correlationId;
        }
    }
}
