using MassTransit.Futures.Contracts;
using Products.Shared.Messages;

namespace Products.Read.API
{
    public static class ProductRepositoryMemberData
    {
        public static IEnumerable<object[]> AddProductValidCommandTestData()
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
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status, quantityOnHand, quantityAvailable, uom, lowStockThreshold);


            return new List<object[]> { new object[] { productAddedMessage } };
        }

        public static IEnumerable<object[]> AddProductInvalidNameInCommandTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            string correlationId = Guid.NewGuid().ToString();
            string productName = null!;
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string status = "Active";
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status, quantityOnHand, quantityAvailable, uom, lowStockThreshold);


            return new List<object[]> { new object[] { productAddedMessage } };
        }

        public static IEnumerable<object[]> AddProductAndUpdateStatusValidCommandsTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            int updatedVersion = 1;
            string correlationId1 = Guid.NewGuid().ToString();
            string correlationId2 = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;
            string initialStatus = "Active";
            string updatedStatus = "InActive";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId1, productName, category, description, price, currency, initialStatus, quantityOnHand, quantityAvailable, uom, lowStockThreshold);
            StatusUpdatedMessage statusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, updatedVersion,
                correlationId2, updatedStatus);

            return new List<object[]> { new object[] { productAddedMessage, statusUpdatedMessage } };
        }

        public static IEnumerable<object[]> AddProductAndUpdateStatusDuplicateProductVersionTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int initialVersion = 3;
            string correlationId = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string initialStatus = "Active";
            int firstUpdatedVersion = 4;                 // version 1 will be missing, should throw MissingProductVersionException
            string firstUpdatedStatus = "InActive";
            int secondUpdatedVersion = 2;
            string secondUpdatedStatus = "Obsolete";
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, initialVersion,
                correlationId, productName, category, description, price, currency, initialStatus, quantityOnHand, quantityAvailable, uom, lowStockThreshold);
            StatusUpdatedMessage firstStatusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, firstUpdatedVersion,
                correlationId, firstUpdatedStatus);
            StatusUpdatedMessage secondStatusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, secondUpdatedVersion,
                correlationId, secondUpdatedStatus);

            return new List<object[]> { new object[] { productAddedMessage, firstStatusUpdatedMessage, secondStatusUpdatedMessage } };
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
            int imageVersion = 1;
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status, quantityOnHand, quantityAvailable, uom, lowStockThreshold);
            ImageAddedMessage imageAddedMessage = new ImageAddedMessage(aggregateId, aggregateType, imageVersion,
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
            int documentVersion = 1;
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId, productName, category, description, price, currency, status, quantityOnHand, quantityAvailable, uom, lowStockThreshold);
            DocumentAddedMessage documentAddedMessage = new DocumentAddedMessage(aggregateId, aggregateType, documentVersion,
                correlationId, documentName, title, sequenceNumber, documentUrl);

            return new List<object[]> { new object[] { productAddedMessage, documentAddedMessage } };
        }


        public static IEnumerable<object[]> AddProductAndUpdateStatusMissingProductVersionTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int aggregateVersion = 0;
            int updatedVersion = 12;    // should throw if version is greater than current + 1 (in this case 1)
            string correlationId1 = Guid.NewGuid().ToString();
            string correlationId2 = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;
            string initialStatus = "Active";
            string updatedStatus = "InActive";

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, aggregateVersion,
                correlationId1, productName, category, description, price, currency, initialStatus, quantityOnHand, quantityAvailable, uom, lowStockThreshold);
            StatusUpdatedMessage statusUpdatedMessage = new StatusUpdatedMessage(aggregateId, aggregateType, updatedVersion,
                correlationId2, updatedStatus);

            return new List<object[]> { new object[] { productAddedMessage, statusUpdatedMessage } };
        }

        public static IEnumerable<object[]> AddProductAndUpdateStatusProductNotFoundTestData()
        {
            Guid aggregateId = Guid.NewGuid();
            string aggregateType = "Product";
            int initialVersion = 0;
            string correlationId1 = Guid.NewGuid().ToString();
            string correlationId2 = Guid.NewGuid().ToString();
            string productName = "Meade LX8";
            string category = "Astronomy";
            string description = "Catadioptric Telescope";
            decimal price = 1299.99m;
            string currency = "USD";
            string initialStatus = "Active";
            int updatedVersion = 1;
            string updatedStatus = "InActive";
            Guid incorrectAggregateId = Guid.NewGuid();
            int quantityOnHand = 1;
            int quantityAvailable = 1;
            string uom = "each";
            int lowStockThreshold = 1;

            ProductAddedMessage productAddedMessage = new ProductAddedMessage(aggregateId, aggregateType, initialVersion,
                correlationId1, productName, category, description, price, currency, initialStatus, quantityOnHand, quantityAvailable, uom, lowStockThreshold);
            StatusUpdatedMessage statusUpdatedMessage = new StatusUpdatedMessage(incorrectAggregateId, aggregateType, updatedVersion,
                correlationId2, updatedStatus);

            return new List<object[]> { new object[] { productAddedMessage, statusUpdatedMessage } };
        }

    }
}
