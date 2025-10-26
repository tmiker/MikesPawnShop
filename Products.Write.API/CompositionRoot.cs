using Microsoft.EntityFrameworkCore;
using Products.Write.API.Configuration;
using Products.Write.API.ExceptionHandling.ExceptionHandlers;
using Products.Write.Application;
using Products.Write.Infrastructure;
using Products.Write.Infrastructure.DataAccess;

namespace Products.Write.API
{
    public static class CompositionRoot
    {
        public static IServiceCollection ComposeApplication(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddDbContext<EventStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetValue<string>("ProductEventStoreSettings:LocalDevelopmentConnectionString"));
            });

            services.AddOptions<CloudAMQPSettings>().Configure<IConfiguration>((options, config) =>
            {
                config.GetSection(nameof(CloudAMQPSettings)).Bind(options);
            });

            // Register exception handlers in order of specificity (most specific first)
            services.AddExceptionHandler<ValidationExceptionHandler>();
            services.AddExceptionHandler<NotFoundExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>(); // Backup handler

            // Register Class Library services
            services.RegisterInfrastructureServices();
            services.RegisterApplicationServices();

            return services;
        }
    }
}
