using System.ComponentModel.DataAnnotations.Schema;

namespace Development.Blazor.DTOs.Read
{
    public class ImageDataDTO
    {
        public int Id { get; init; }
        public string? Name { get; init; }           
        public string? Caption { get; init; }
        public int SequenceNumber { get; init; }
        public string? ImageUrl { get; init; }
        public string? ThumbnailUrl { get; init; }
        public int ProductId { get; init; }
    }
}
