﻿using MassTransit;
using Products.Read.API.Abstractions;
using Products.Shared.Messages;

namespace Products.Read.API.MessageConsumers
{
    public class ImageAddedConsumer : IConsumer<ImageAddedMessage>
    {
        private readonly IProductMessageProcessor _messageProcessor;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ImageAddedConsumer> _logger;

        public ImageAddedConsumer(IProductMessageProcessor messageProcessor, IProductRepository productRepository, ILogger<ImageAddedConsumer> logger)
        {
            _messageProcessor = messageProcessor;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ImageAddedMessage> context)
        {
            var message = context.Message;
            _logger.LogInformation("Image Added Message Received: VERSION = {version}, AggregateId = {message.AggregateId}, " +
                "Caption = {message.Caption}", message.AggregateVersion, message.AggregateId, message.Caption);

            // await _productRepository.AddProductImageAsync(message);

            bool messagesInMessageRecordQueue = await _messageProcessor.ProcessProductMessageAsync(message);

            // really want to batch process messages and call the below after processing a batch, or something equivalent
            if (messagesInMessageRecordQueue) await _messageProcessor.ProcessMessageRecordsFromQueue();
        }
    }
}
