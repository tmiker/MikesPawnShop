using MassTransit.Futures.Contracts;
using Products.Shared.Messages;

namespace Products.Read.API
{
    public static class ProductRepositoryMemberData
    {
        public static IEnumerable<object[]> AddProductCommandTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string status = "Active";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);

            return new List<object[]> { new object[] { productAddedMessage } };
        }

        public static IEnumerable<object[]> AddProductAndUpdateStatusCommandsTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string initialStatus = "Active";
            string updatedStatus = "InActive";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, initialStatus);
            StatusUpdatedMessage statusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, updatedStatus);

            return new List<object[]> { new object[] { productAddedMessage, statusUpdatedMessage } };
        }

        public static IEnumerable<object[]> AddProductAndAddImageCommandsTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string status = "Active";
            string imageName = "Telescope";
            string caption = "Meade LX8";
            int sequenceNumber = 1;
            string imageUrl = "https://www.docs.imageUrl";
            string thumbUrl = "https://www.docs.thumbUrl";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);
            ImageAddedMessage imageAddedMessage = new ImageAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, imageName, caption, sequenceNumber, imageUrl, thumbUrl);

            return new List<object[]> { new object[] { productAddedMessage, imageAddedMessage } };
        }

        public static IEnumerable<object[]> AddProductAndAddDocumentCommandsTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string status = "Active";
            string documentName = "Instructions";
            string title = "Meade LX8 Instructions";
            int sequenceNumber = 1;
            string documentUrl = "https://www.docs.documentUrl";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status);
            DocumentAddedMessage documentAddedMessage = new DocumentAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, documentName, title, sequenceNumber, documentUrl);

            return new List<object[]> { new object[] { productAddedMessage, documentAddedMessage } };
        }
    }
}
