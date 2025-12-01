using System.Text.Json.Serialization;

namespace Admin.Blazor.Client.ErrorHandling
{
    public class CustomProblemDetails
    {
        public string? Type { get; set; }
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? Detail { get; set; }
        public string? Instance { get; set; }

        //public IDictionary<string, string[]>? Errors { get; set; }

        [JsonExtensionData]
        public IDictionary<string, object?>? Extensions { get; set; }

    }
}
