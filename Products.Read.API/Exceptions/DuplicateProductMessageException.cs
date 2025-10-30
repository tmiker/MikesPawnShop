namespace Products.Read.API.Exceptions
{
    public class DuplicateProductMessageException : Exception
    {
        public DuplicateProductMessageException(string message) : base(message) { }
    }
}
