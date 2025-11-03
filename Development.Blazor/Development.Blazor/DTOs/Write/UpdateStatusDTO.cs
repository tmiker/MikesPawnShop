namespace Development.Blazor.DTOs.Write
{
    public class UpdateStatusDTO
    {
        public Guid ProductId { get; set; }
        public string Status { get; set; } = default!;

        public UpdateStatusDTO(Guid productId, string status)
        {
            ProductId = productId;
            Status = status;
        }
    }
}
