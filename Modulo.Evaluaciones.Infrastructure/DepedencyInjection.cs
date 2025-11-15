namespace Modulo.Evaluaciones.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddEvaluacionesModule(this IServiceCollection services, IConfiguration cfg)
        {
            var cs = cfg.GetConnectionString("DefaultConnection")
                     ?? cfg["SqlServer:ConnectionString"]
                     ?? throw new InvalidOperationException("Connection string not found.");

            services.AddSingleton(new SqlOptions(cs));

            services.AddScoped<IEvaluacionesRepository, EvaluacionesRepository>();
            services.AddScoped<IndicadoresCatalogoService>();
            services.AddScoped<PlantillasQueryService>();
            services.AddScoped<PlantillasCommandService>();
            services.AddScoped<CrearEvaluacionService>();
            services.AddScoped<ActualizarEvaluacionService>();
            services.AddScoped<ListarEvaluacionesService>();
            services.AddScoped<ObtenerDetalleEvaluacionService>();
            services.AddScoped<ObtenerMisEvaluacionesService>();

            return services;
        }
    }

  
}
