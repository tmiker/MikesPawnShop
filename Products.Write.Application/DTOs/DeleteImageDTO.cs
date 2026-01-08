using System.ComponentModel.DataAnnotations;

namespace Products.Write.Application.DTOs
{
    public class DeleteImageDTO
    {
        public string ProductId { get; init; }
        public string FileName { get; init; }

        public DeleteImageDTO(string productId, string fileName)
        {
            ProductId = productId;
            FileName = fileName;
        }
    }
}
