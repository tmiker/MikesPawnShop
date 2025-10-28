using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandHandlers;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.CQRS.DevTests;
using Products.Write.Application.CQRS.Dispatchers;
using Products.Write.Application.EventManagement;
using System.Security.Authentication;

namespace Products.Write.Application
{
    public static class DIRegistrations
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            // Register CloudAMQP related services
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            string amqpUrl = configuration.GetSection("CloudAMQPSettings:Url").Value ?? throw new ArgumentNullException("Invalid Cloud AMQP configuration.");
            string amqpUsername = configuration.GetSection("CloudAMQPSettings:UserVhost").Value ?? throw new ArgumentNullException("Invalid Cloud AMQP configuration.");
            string amqpPassword = configuration.GetSection("CloudAMQPSettings:Password").Value ?? throw new ArgumentNullException("Invalid Cloud AMQP configuration.");

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(amqpUrl), h =>
                    {
                        h.Username(amqpUsername);
                        h.Password(amqpPassword);

                        h.UseSsl(s =>
                        {
                            s.Protocol = SslProtocols.Tls12;
                        });
                    });
                });
            });

            // Register the SingleThreadedEventAggregator as a singleton
            services.AddScoped<SingleThreadedEventAggregator>();
            // Register ProductEventHandlers 
            services.AddScoped<ProductEventHandlers>();

            // Build the service provider and register the event handlers with the event aggregator as IEventAggregator
            // var serviceProvider = services.BuildServiceProvider();
            services.AddScoped(serviceProvider =>
            {
                IRegisterableEventHandlers handlers = serviceProvider.GetRequiredService<ProductEventHandlers>();
                IEventAggregator aggregator = serviceProvider.GetRequiredService<SingleThreadedEventAggregator>();
                aggregator.Register(handlers);
                return aggregator;
            });

            // Register Dispatchers
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            // Register Command Handlers
            services.AddScoped<ICommandHandler<AddProduct, AddProductResult>, AddProductHandler>();

            // Register Query Handlers

            // Register Dev Test Handlers
            services.AddScoped<ICommandHandler<ThrowException, ThrowExceptionResult>, ThrowExceptionHandler>();
            services.AddScoped<ICommandHandler<ProcessMultipleEvents, ProcessMultipleEventsResult>, ProcessMultipleEventsHandler>();

            return services;
        }
    }
}
