using Microsoft.Extensions.Primitives;
using Products.Write.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.CQRS.Commands
{
    public class UpdateStatus
    {
        [Required]
        public Guid ProductId { get; init; }
        [Required]
        public string Status { get; init; } = string.Empty;
        public string? CorrelationId { get; set; }


        public UpdateStatus(UpdateStatusDTO updateStatusDTO, StringValues correlationId)
        {
            ProductId = updateStatusDTO.ProductId;
            Status = updateStatusDTO.Status;
            CorrelationId = correlationId;
        }
    }
}