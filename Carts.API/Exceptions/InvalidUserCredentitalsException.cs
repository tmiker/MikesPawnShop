namespace Carts.API.Exceptions
{
    public class InvalidUserCredentitalsException : Exception
    {
        public InvalidUserCredentitalsException(string message) : base(message) { }
        public InvalidUserCredentitalsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
