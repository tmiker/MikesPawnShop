namespace Products.Write.Application.CQRS.Commands
{
    public class UpdateStatus
    {
        public Guid ProductId { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? CorrelationId { get; set; }
    }
}
