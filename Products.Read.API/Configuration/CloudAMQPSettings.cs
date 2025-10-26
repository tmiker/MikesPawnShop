namespace Products.Read.API.Configuration
{
    public class CloudAMQPSettings
    {
        public const string SectionName = "CloudAMQPSettings";
        public string? UserVhost { get; init; }
        public string? Password { get; init; }
        public int DefaultPort { get; init; }
        public int TlsPort { get; init; }
        public string? Url { get; init; }
    }
}
