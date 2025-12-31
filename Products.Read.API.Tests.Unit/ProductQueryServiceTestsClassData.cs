using Products.Read.API.Domain.Models;
using System.Collections;

namespace Products.Read.API
{
    public class ProductQueryServiceTestsClassData : IEnumerable<object[]>
    {
        static Guid aggregateId = Guid.NewGuid();
        static string aggregateType = "Product";
        static int aggregateVersion = 0;
        static string correlationId = Guid.NewGuid().ToString();
        static string productName = "Meade LX8";
        static string category = "Astronomy";
        static string description = "Catadioptric Telescope";
        static decimal price = 1299.99m;
        static string currency = "USD";
        static string status = "Active";
        static string imageName = "Telescope";
        static string caption = "Meade LX8";
        static int imageSequenceNumber = 1;
        static string imageUrl = "https://www.docs.imageUrl";
        static string thumbUrl = "https://www.docs.thumbUrl";
        static int imageVersion = 1;   // should be aggregateVersion + 1
        static string documentName = "Instructions";
        static string title = "Meade LX8 Instructions";
        static int documentSequenceNumber = 1;
        static string documentUrl = "https://www.docs.documentUrl";
        static int documentVersion = 2;    // should be aggregateVersion + 2
        static int quantityOnHand = 1;
        static int quantityAvailable = 1;
        static string uom = "each";
        static int lowStockThreshold = 1;

        public Product product { get; set; } = new Product(aggregateId, productName, category, description, price, currency, status, quantityOnHand, quantityAvailable, uom, lowStockThreshold, aggregateVersion);
        ImageData image = new ImageData(imageName, caption, imageSequenceNumber, imageUrl, thumbUrl);
        DocumentData document = new DocumentData(documentName, title, documentSequenceNumber, documentUrl);

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { product, image, document };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
