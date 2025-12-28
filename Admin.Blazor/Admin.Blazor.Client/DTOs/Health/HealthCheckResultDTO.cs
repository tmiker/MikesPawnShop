namespace Admin.Blazor.Client.DTOs.Health
{
    public class HealthCheckResultDTO
    {
        public string? Status { get; set; }
        public string? TotalDuration { get; set; }
        public Dictionary<string, HealthCheckResultEntriesDTO>? Entries { get; set; }
    }
}
