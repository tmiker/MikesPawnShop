namespace Products.Write.Application.CQRS.Commands
{
    public class AddImage
    {
        public Guid ProductId { get; init; }
        public string Name { get; init; } = string.Empty;       // virtual directory plus filename
        public string Caption { get; init; } = string.Empty;
        public int SequenceNumber { get; init; }
        public string ImageUrl { get; init; } = string.Empty;
        public string ThumbnailUrl { get; init; } = string.Empty;
        public string? CorrelationId { get; set; } 
    }
}
