namespace Products.Read.API.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message) { }
        public NotFoundException(string name, object key)
            : base($"Entity '{name}' with key '{key}' was not found.") { }
        public override int StatusCode => StatusCodes.Status404NotFound;
        public override string ErrorType => "NotFound";
    }
}
