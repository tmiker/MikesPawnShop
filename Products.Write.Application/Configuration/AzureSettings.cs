namespace Products.Write.Application.Configuration
{
    public class AzureSettings
    {
        public const string SectionName = "AzureSettings";
        public string? BlobStorageConnectionString { get; init; }
    }
}
