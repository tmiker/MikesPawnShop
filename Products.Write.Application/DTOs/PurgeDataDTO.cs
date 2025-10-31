using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.DTOs
{
    public class PurgeDataDTO
    {
        [Required]
        [DisplayName("Pin Number")]
        public int PinNumber { get; init; }
    }
}
