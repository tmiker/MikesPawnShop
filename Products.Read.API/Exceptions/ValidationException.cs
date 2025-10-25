namespace Products.Read.API.Exceptions
{
    public class ValidationException : BaseException
    {
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
        public override int StatusCode => StatusCodes.Status400BadRequest;
        public override string ErrorType => "ValidationError";
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
    }
}
