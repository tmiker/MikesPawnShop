using Microsoft.Extensions.DependencyInjection;
using Products.Write.Infrastructure.Abstractions;
using Products.Write.Infrastructure.EventManagement;
using Products.Write.Infrastructure.EventStores;
using Products.Write.Infrastructure.Repositories;

namespace Products.Write.Infrastructure
{
    public static class DIRegistrations
    {
        public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
        {
            // Register Product Event Store
            services.AddScoped<IProductEventStore, ProductEventStore>();
            // Register Product Repository
            services.AddScoped<IProductRepository, ProductRepository>();

            // Register the SingleThreadedEventAggregator as a singleton
            services.AddSingleton<SingleThreadedEventAggregator>();
            // Register ProductEventHandlers as a singleton
            services.AddSingleton<ProductEventHandlers>();

            // Build the service provider and register the event handlers with the event aggregator as IEventAggregator
            var serviceProvider = services.BuildServiceProvider();
            //var eventAggregator = serviceProvider.GetRequiredService<IEventAggregator>();
            //var productEventHandlers = serviceProvider.GetRequiredService<IRegisterableEventHandlers>();
            //eventAggregator.Register(productEventHandlers);
            services.AddSingleton(serviceProvider =>
            {
                IRegisterableEventHandlers handlers = serviceProvider.GetRequiredService<ProductEventHandlers>();
                IEventAggregator aggregator = serviceProvider.GetRequiredService<SingleThreadedEventAggregator>();
                aggregator.Register(handlers);
                return aggregator;
            });

            return services;
        }
    }
}
