namespace Modulo.Empleados.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEmpleadosInfraestructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<EmpleadosDbContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
            services.AddScoped<IEmpleadoStoredRepository, EmpleadoStoredRepository>();
            services.AddScoped<EmpleadoQueryService>();

            return services;
        }
    }
}
