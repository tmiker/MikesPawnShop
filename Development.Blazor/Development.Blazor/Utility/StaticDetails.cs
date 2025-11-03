using System.Runtime.CompilerServices;

namespace Development.Blazor.Utility
{
    public class StaticDetails
    {
        public const int PurgeDataPinNumber = 7453;

        public const string ProductsReadHttpClient_ClientName = "ProductsReadHttpClient";
        public const string ProductsReadHttpClient_BaseURL = "https://localhost:7101";
        public const string ProductsReadHttpClient_ProductsPath = "/api/products";
        public const string ProductsReadHttpClient_DevTestsPath = "/api/devTests";

        public const string ProductsWriteHttpClient_ClientName = "ProductsWriteHttpClient";
        public const string ProductsWriteHttpClient_BaseURL = "https://localhost:7213";
        public const string ProductsWriteHttpClient_ProductsPath = "/api/productsManagement";
        public const string ProductsWriteHttpClient_DevTestsPath = "/api/devTests";


    }
}
