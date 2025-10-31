using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.CQRS.DevTests
{
    public class PurgeData
    {
        [Required]
        [DisplayName("Pin Number")]
        public int PinNumber { get; init; }
        public string? CorrelationId { get; set; } = default!;

        public PurgeData(int pinNumber, string? correlationId)
        {
            PinNumber = pinNumber;
            CorrelationId = correlationId;
        }
    }
}
