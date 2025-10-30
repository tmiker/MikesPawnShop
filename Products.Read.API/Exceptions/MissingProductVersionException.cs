namespace Products.Read.API.Exceptions
{
    public class MissingProductVersionException : BaseException
    {
        public MissingProductVersionException(string message) : base(message) { }
        public override int StatusCode => StatusCodes.Status422UnprocessableEntity;
        public override string ErrorType => "Missing Product Version";
    }
}
