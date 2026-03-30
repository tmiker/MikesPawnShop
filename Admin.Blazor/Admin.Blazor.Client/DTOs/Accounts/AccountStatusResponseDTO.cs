namespace Admin.Blazor.Client.DTOs.Accounts
{
    public class AccountStatusResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Status { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
