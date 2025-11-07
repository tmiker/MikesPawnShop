using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Products.Write.Application.DTOs;
using System.Text;

namespace Products.Write.Application.CQRS.Commands
{
    public class AddDocument
    {
        public Guid ProductId { get; init; }
        public string Name { get; private set; } = default!;          // for Azure blob storage, virtual directory plus filename
        public string Title { get; init; } = default!;
        public int SequenceNumber { get; init; }
        public string? CorrelationId { get; set; }
        public IFormFile? DocumentBlob { get; set; }
        public string? BlobFileName { get; set; }

        public AddDocument(AddDocumentDTO addDocumentDTO, StringValues correlationId)
        {
            ProductId = Guid.Parse(addDocumentDTO.ProductId);
            Name = addDocumentDTO.Name;
            Title = addDocumentDTO.Title;
            SequenceNumber = addDocumentDTO.SequenceNumber;
            CorrelationId = correlationId;
            DocumentBlob = addDocumentDTO.DocumentBlob;
            BlobFileName = addDocumentDTO.BlobFileName;
        }

        public void CleanFileName()
        {
            // best performance is using string builder loop vs using regex
            if (string.IsNullOrEmpty(Name))
            {
                string randomFileName = Path.GetRandomFileName();
                string randomName = Path.GetFileNameWithoutExtension(randomFileName);
                Name = randomName;
            }
            StringBuilder sb = new StringBuilder();
            foreach (char c in Name)
            {
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == '-' || c == '_') sb.Append(c);
            }
            Name = sb.ToString();
        }
    }
}
