using Microsoft.Extensions.Primitives;
using Products.Write.Application.DTOs;
using Products.Write.Domain.Enumerations;

namespace Products.Write.Application.CQRS.DevTests
{
    public class ThrowException
    {
        public string ExceptionType { get; init; } = default!;
        public string? CorrelationId { get; set; } = default!;

        public ThrowException(string exceptionType, string? correlationId)
        {
            ExceptionType = exceptionType;
            CorrelationId = correlationId;
        }

        public ThrowException(ThrowExceptionDTO dto, StringValues correlationId)
        {
            ExceptionType = dto.ExceptionType;
            CorrelationId = correlationId;
        }
    }
}
