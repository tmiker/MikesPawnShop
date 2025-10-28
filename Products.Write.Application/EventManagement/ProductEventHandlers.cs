﻿using MassTransit;
using Products.Shared.Messages;
using Products.Write.Application.Abstractions;
using Products.Write.Domain.Events;
using System.Threading.Tasks;

namespace Products.Write.Application.EventManagement
{
    public class ProductEventHandlers : IRegisterableEventHandlers
    {
        public const string AggregateType = "Product";
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductEventHandlers(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public void RegisterWithEventAggregator(IEventAggregator eventAggregator)
        {
            eventAggregator.Register<ProductAdded>(OnProductAdded);
            eventAggregator.Register<StatusUpdated>(OnStatusUpdated);
            eventAggregator.Register<ImageAdded>(OnImageAdded);
            eventAggregator.Register<DocumentAdded>(OnDocumentAdded);
        }

        private void OnProductAdded(ProductAdded @event)
        {
            Console.WriteLine($"Product Added: {@event.Name}, Category: {@event.Category}, Price: {@event.Price} {@event.Currency}");
            ProductAddedMessage message = new ProductAddedMessage(
                @event.AggregateId,
                @event.AggregateType,
                @event.AggregateVersion,
                @event.CorrelationId,
                @event.Name,
                @event.Category,
                @event.Description,
                @event.Price,
                @event.Currency,
                @event.Status
            );
            _publishEndpoint.Publish(message);
        }
        private void OnStatusUpdated(StatusUpdated @event)
        {
            Console.WriteLine($"Product Status Updated: {@event.AggregateId}, New Status: {@event.Status}");
            StatusUpdatedMessage message = new StatusUpdatedMessage(
                @event.AggregateId,
                @event.AggregateType,
                @event.AggregateVersion,
                @event.CorrelationId,
                @event.Status
            );
            _publishEndpoint.Publish(message);
        }
        private void OnImageAdded(ImageAdded @event)
        {
            Console.WriteLine($"Image Added to Product: {@event.AggregateId}, Image URL: {@event.ImageUrl}");
            ImageAddedMessage message = new ImageAddedMessage(
                @event.AggregateId,
                @event.AggregateType,
                @event.AggregateVersion,
                @event.CorrelationId,
                @event.Name,
                @event.Caption,
                @event.SequenceNumber,
                @event.ImageUrl,
                @event.ThumbnailUrl
            );
            _publishEndpoint.Publish(message);
        }
        private void OnDocumentAdded(DocumentAdded @event)
        {
            Console.WriteLine($"Document Added to Product: {@event.AggregateId}, Document URL: {@event.DocumentUrl}");
            DocumentAddedMessage message = new DocumentAddedMessage(
                @event.AggregateId,
                @event.AggregateType,
                @event.AggregateVersion,
                @event.CorrelationId,
                @event.Name,
                @event.Title,
                @event.SequenceNumber,
                @event.DocumentUrl
            );
            _publishEndpoint.Publish(message);
        }
    }
}
