using Products.Read.API.Configuration;

namespace Products.Read.API
{
    public static class CompositionRoot
    {
        public static IServiceCollection ComposeApplication(this IServiceCollection services)
        {
            services.AddOptions<CloudAMQPSettings>().Configure<IConfiguration>((options, config) =>
            {
                config.GetSection(nameof(CloudAMQPSettings)).Bind(options);
            });

            

            return services;
        }
    }
}
