namespace Products.Write.API.ExceptionHandling.Exceptions
{
    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message) : base(message) { }
        public override int StatusCode => StatusCodes.Status403Forbidden;
        public override string ErrorType => "Forbidden";
    }
}
