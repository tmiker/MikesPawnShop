﻿using Products.Write.Domain.Base;

namespace Products.Write.Domain.ValueObjects
{
    public class ImageData : ValueObject
    {
        public string? Name { get; private set; }           // virtual directory plus filename
        public string? Caption { get; private set; }
        public int SequenceNumber { get; private set; }
        public string? ImageUrl { get; private set; }
        public string? ThumbnailUrl { get; private set; }

        public ImageData() { }

        public ImageData(string name, string caption, int sequenceNumber, string imageUrl, string thumbnailUrl)
        {
            Name = name;
            Caption = caption;
            SequenceNumber = sequenceNumber;
            ImageUrl = imageUrl;
            ThumbnailUrl = thumbnailUrl;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Name!;
            yield return Caption!;
            yield return SequenceNumber;
            yield return ImageUrl!;
            yield return ThumbnailUrl!;
        }
    }
}
