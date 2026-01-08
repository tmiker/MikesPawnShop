namespace Products.Write.Application.DTOs
{
    public class UpdateStatusDTO
    {
        public string? ProductId { get; set; }
        public string? Status { get; set; } 

        //public UpdateStatusDTO(Guid productId, string status)
        //{
        //    ProductId = productId;
        //    Status = status;
        //}
    }
}
