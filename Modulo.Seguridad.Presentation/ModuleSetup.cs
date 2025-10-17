namespace Modulo.Seguridad.Presentation
{
    public static class ModuleSetup
    {
        public static IServiceCollection AddSeguridadModule(this IServiceCollection services, IConfiguration config)
        {
            // registrar la infraestructura
            services.AddSeguridadInfrastructure(config);

            // registrar casos de uso
            services.AddScoped<IAuthUseCase, AuthUseCase>();

            return services;
        }
    }
}
