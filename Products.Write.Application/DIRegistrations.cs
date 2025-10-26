using Microsoft.Extensions.DependencyInjection;
using Products.Write.Application.Abstractions;
using Products.Write.Application.CQRS.CommandHandlers;
using Products.Write.Application.CQRS.CommandResults;
using Products.Write.Application.CQRS.Commands;
using Products.Write.Application.CQRS.DevTests;
using Products.Write.Application.CQRS.Dispatchers;

namespace Products.Write.Application
{
    public static class DIRegistrations
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            // Register Dispatchers
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            // Register Command Handlers
            services.AddScoped<ICommandHandler<AddProduct, AddProductResult>, AddProductHandler>();

            services.AddScoped<ICommandHandler<ThrowException, ThrowExceptionResult>, ThrowExceptionHandler>();

            // Register Query Handlers

            return services;
        }
    }
}
