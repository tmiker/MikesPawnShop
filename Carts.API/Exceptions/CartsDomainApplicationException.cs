namespace Carts.API.Exceptions
{
    public class CartsDomainApplicationException : ApplicationException
    {
        public CartsDomainApplicationException(string message) : base(message) { }
        public CartsDomainApplicationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
