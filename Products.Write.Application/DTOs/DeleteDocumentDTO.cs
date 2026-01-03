using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.DTOs
{
    public class DeleteDocumentDTO
    {
        [Required]
        public string ProductId { get; init; }
        [Required]
        public string FileName { get; init; } 

        public DeleteDocumentDTO(string productId, string fileName)
        {
            ProductId = productId;
            FileName = fileName;
        }
    }
}
