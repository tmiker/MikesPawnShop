using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Blazor.Client.DTOs.Products.Test
{
    public class PurgeDataDTO
    {
        [Required]
        [DisplayName("Pin Number")]
        public int PinNumber { get; set; }
    }
}
