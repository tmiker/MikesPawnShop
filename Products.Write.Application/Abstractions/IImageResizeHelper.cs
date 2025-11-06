namespace Products.Write.Application.Abstractions
{
    public interface IImageResizeHelper
    {
        byte[]? ResizeImage(byte[] original, int maxLength);
    }
}
