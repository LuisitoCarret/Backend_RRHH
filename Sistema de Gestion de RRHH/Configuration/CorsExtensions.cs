namespace Sistema_de_Gestion_de_RRHH.Configuration
{
    public static class CorsExtensions
    {
        public const string DefaultCorsPolicy = "DefaultCors";

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration config)
        {
            var allowed = config.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
            var allowAnyDev = config.GetValue<bool>("Cors:AllowAnyOriginDev");

            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy, policy =>
                {
                    if (allowed.Length > 0)
                    {
                        policy.WithOrigins(allowed)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    }
                    else if (allowAnyDev)
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseDefaultCors(this IApplicationBuilder app)
        {
            return app.UseCors(DefaultCorsPolicy);
        }
    }
}
