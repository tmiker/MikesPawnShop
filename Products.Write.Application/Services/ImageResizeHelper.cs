using ImageMagick;
using Microsoft.Extensions.Logging;
using Products.Write.Application.Abstractions;

namespace Products.Write.Application.Services
{
    public class ImageResizeHelper : IImageResizeHelper
    {
        private readonly ILogger<ImageResizeHelper> _logger;

        public ImageResizeHelper(ILogger<ImageResizeHelper> logger)
        {
            _logger = logger;
        }

        public byte[]? ResizeImage(byte[] original, int maxLength)
        {
            if (original != null)
            {
                using (var resized = new MagickImage(original))
                {
                    var size = new MagickGeometry((uint)maxLength, (uint)maxLength);
                    size.IgnoreAspectRatio = false;

                    resized.Resize(size);
                    //resized.Write("some filepath");     		// writes file to the specified filepath - not required

                    byte[] thumbnail = resized.ToByteArray();

                    Console.WriteLine($"*** Resize Image exiting successfully ***");
                    return thumbnail;
                }
            }
            return null;
        }
    }
}
