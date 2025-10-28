namespace Products.Write.Application.CQRS.Commands
{
    public class AddDocument
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = string.Empty;          // for Azure blob storage, virtual directory plus filename
        public string Title { get; init; } = string.Empty;
        public int SequenceNumber { get; init; }
        public string DocumentUrl { get; init; } = string.Empty;
        public string? CorrelationId { get; set; } 
    }
}
