using MediatR;
using Microsoft.Extensions.Primitives;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.CQRS.Commands
{
    public class UpdateStatus : IRequest<UpdateStatusResult>
    {
        [Required]
        public Guid ProductId { get; init; }
        [Required]
        public string Status { get; init; } = string.Empty;
        public string? CorrelationId { get; set; }


        public UpdateStatus(UpdateStatusDTO updateStatusDTO, StringValues correlationId)
        {
            ProductId = !string.IsNullOrWhiteSpace(updateStatusDTO.ProductId) ? Guid.Parse(updateStatusDTO.ProductId) : throw new ArgumentNullException(nameof(updateStatusDTO.ProductId));
            Status = !string.IsNullOrWhiteSpace(updateStatusDTO.Status) ? updateStatusDTO.Status : throw new ArgumentNullException(nameof(updateStatusDTO.Status));
            CorrelationId = correlationId;
        }
    }
}