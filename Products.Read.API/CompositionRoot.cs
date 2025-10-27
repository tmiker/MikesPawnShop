using Microsoft.EntityFrameworkCore;
using Products.Read.API.Configuration;
using Products.Read.API.Infrastructure.Data;

namespace Products.Read.API
{
    public static class CompositionRoot
    {
        public static IServiceCollection ComposeApplication(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            services.AddDbContext<ProductsReadDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LocalDevelopmentConnectionString"));
            });

            services.AddOptions<CloudAMQPSettings>().Configure<IConfiguration>((options, config) =>
            {
                config.GetSection(nameof(CloudAMQPSettings)).Bind(options);
            });

            return services;
        }
    }
}
