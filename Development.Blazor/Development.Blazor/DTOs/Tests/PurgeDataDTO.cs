using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Development.Blazor.DTOs.Tests
{
    public class PurgeDataDTO
    {
        [Required]
        [DisplayName("Pin Number")]
        public int PinNumber { get; set; }
    }
}
