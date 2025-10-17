namespace Modulo.Seguridad.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSeguridadInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            var csFromConfig = config?.GetConnectionString("DefaultConnection");

            if (!string.IsNullOrWhiteSpace(csFromConfig))
            {
                services.AddScoped<IUsuarioRepository>(sp => new UsuarioRepository(csFromConfig));
                return services;
            }

            services.AddScoped<IUsuarioRepository>(sp =>
            {
                var cfg = sp.GetRequiredService<IConfiguration>();
                var cs = cfg.GetConnectionString("DefaultConnection")
                         ?? throw new InvalidOperationException("DefaultConnection not found in IConfiguration.");
                return new UsuarioRepository(cs);
            });

            return services;
        }
    }
}
