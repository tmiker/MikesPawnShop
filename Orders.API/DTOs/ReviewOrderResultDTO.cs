namespace Orders.API.DTOs
{
    public class ReviewOrderResultDTO
    {
        //// public OrderDTO? OrderToReview { get; set; }
        //public AccountDetailDTO? AccountDetail { get; set; }
        //public ShoppingCartDTO? ShoppingCart { get; set; }

        //// dev test only
        //public string? AccountOwnerId { get; set; }
        //public string? CartOwnerId { get; set; }

        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
