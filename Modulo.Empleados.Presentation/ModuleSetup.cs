namespace Modulo.Empleados.Presentation
{
    public static class ModuleSetup
    {
        public static IServiceCollection AddEmpleadosModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEmpleadosInfraestructure(configuration);
            services.AddScoped<EmpleadoService>();
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<EmpleadoMappingProfile>();
            });

            return services;
        }
    }
}
