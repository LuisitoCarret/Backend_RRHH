// ==========================
// Modulo.Reclutamiento.Infrastructure/DependencyInjection.cs
// ==========================
namespace Modulo.Reclutamiento.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddReclutamientoModule(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("DefaultConnection")
                 ?? cfg["Sql:ConnectionString"]
                 ?? cfg["SqlServer:ConnectionString"]
                 ?? throw new Exception("ConnectionString not found for Reclutamiento.");

        services.AddSingleton(new SqlOptions(cs));
        services.AddScoped<IVacanteRepository, VacanteRepository>();
        services.AddScoped<Modulo.Reclutamiento.Application.Services.VacantesService>();
        services.AddScoped<IPostulacionRepository, PostulacionRepository>();
        services.AddScoped<PostulacionesService>();
        services.Configure<CloudinaryOptions>(
            cfg.GetSection("Cloudinary")
        );

        // Cloudinary Service
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        return services;
    }
}
