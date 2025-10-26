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

            services.AddProblemDetails();
            //// ProblemDetails service - configure globally if not using extensions in middleware
            //services.AddProblemDetails(options =>
            //{
            //    // Customize problem details globally
            //    options.CustomizeProblemDetails = (context) =>
            //    {
            //        context.ProblemDetails.Extensions["machine"] = Environment.MachineName;
            //        context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
            //        // Add correlation ID if available
            //        if (context.HttpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationId))
            //        {
            //            context.ProblemDetails.Extensions["correlationId"] = correlationId.ToString();
            //        }
            //    };
            //});

            return services;
        }
    }
}
