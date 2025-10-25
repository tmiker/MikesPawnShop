namespace Products.Read.API.Exceptions
{
    public abstract class BaseException : Exception
    {
        protected BaseException(string message) : base(message) { }
        protected BaseException(string message, Exception innerException) : base(message, innerException) { }

        public abstract int StatusCode { get; }
        public abstract string ErrorType { get; }
    }
}
