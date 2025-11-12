namespace Carts.API.Exceptions
{
    public class CartsDomainException : Exception
    {
        public CartsDomainException(string message) : base(message) { }
        public CartsDomainException(string message, Exception innerException) : base(message, innerException) { }
    }
}
