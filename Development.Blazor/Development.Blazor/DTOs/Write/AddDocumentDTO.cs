using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace Development.Blazor.DTOs.Write
{
    public class AddDocumentDTO
    {
        public string ProductId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int SequenceNumber { get; set; }
        public IBrowserFile? DocumentBlob { get; set; } 
        public string? BlobFileName { get; set; }



        //// PREVIOUS
        //public Guid ProductId { get; init; }
        //public string Name { get; init; } = default!;
        //public string Title { get; init; } = default!;
        //public int SequenceNumber { get; init; }
        //public string DocumentUrl { get; init; } = default!;

        //public AddDocumentDTO(Guid productId, string name, string title, int sequenceNumber, string documentUrl)
        //{
        //    ProductId = productId;
        //    Name = name;
        //    Title = title;
        //    SequenceNumber = sequenceNumber;
        //    DocumentUrl = documentUrl;
        //}
    }
}
