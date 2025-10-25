namespace Products.Write.API.ExceptionHandling.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message) : base(message) { }
        public override int StatusCode => StatusCodes.Status409Conflict;
        public override string ErrorType => "Conflict";
    }
}
