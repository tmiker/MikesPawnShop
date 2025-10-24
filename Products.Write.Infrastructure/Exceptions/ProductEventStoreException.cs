namespace Products.Write.Infrastructure.Exceptions
{
    public class ProductEventStoreException : Exception
    {
        public ProductEventStoreException(string message) : base(message) { }

        public ProductEventStoreException(string message, Exception innerException) : base(message, innerException) { }
    }
}
