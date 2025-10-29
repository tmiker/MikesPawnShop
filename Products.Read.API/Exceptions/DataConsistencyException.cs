namespace Products.Read.API.Exceptions
{
    public class DataConsistencyException : BaseException
    {
        public DataConsistencyException(string message) : base(message) 
        { }

        public override int StatusCode => StatusCodes.Status422UnprocessableEntity;
        public override string ErrorType => "Unprocessable Entity";
    }
}
