using Microsoft.AspNetCore.Mvc;

namespace Products.Write.API.ExceptionHandling.PendingIncorporation.Models
{
    public class ApiProblemDetails : ProblemDetails
    {
        public string? TraceId { get; set; }
        public DateTime Timestamp { get; set; }
        public string? CorrelationId { get; set; }
        public Dictionary<string, object>? Context { get; set; }
        public List<ApiError>? Errors { get; set; }
    }
}
