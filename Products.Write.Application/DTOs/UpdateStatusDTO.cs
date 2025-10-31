namespace Products.Write.Application.DTOs
{
    public class UpdateStatusDTO
    {
        public Guid ProductId { get; init; }
        public string Status { get; init; } = default!;
    }
}
