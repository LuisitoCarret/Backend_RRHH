namespace Modulo.Seguridad.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSeguridadInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            var cs = config.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no fue encontrada.");

            // 🔹 Registrar DbContext
            services.AddDbContext<SeguridadDbContext>(options =>
                options.UseSqlServer(cs));

            // 🔹 Registrar repositorios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioEmpleadoRepository, UsuarioEmpleadoRepository>();

            // 🔹 Registrar servicios de aplicación
            services.AddScoped<IUserCreationService, UserCreationService>();

            return services;
        }
    }
}
