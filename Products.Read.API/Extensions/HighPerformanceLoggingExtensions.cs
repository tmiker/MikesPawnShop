namespace Products.Read.API.Extensions
{
    public static partial class HighPerformanceLoggingExtensions
    {
        [LoggerMessage(EventId = 1000, Level = LogLevel.Information, Message = "Request Method: {method}, Request Path: {path}, CorrelationId: {correlationId}, Found in Request Header: {presentInRequestHeader}")]
        public static partial void CorrelationIdMiddlewareExecuted(this ILogger logger, string? method, string? path, string? correlationId, bool presentInRequestHeader);    //, DateTime time);

    }
}
