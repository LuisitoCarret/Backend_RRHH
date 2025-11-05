namespace Modulo.Asistencias.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAsistenciasModule(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("DefaultConnection")
                 ?? cfg["SqlServer:ConnectionString"]
                 ?? throw new InvalidOperationException("Connection string not found.");

        services.AddSingleton(new SqlOptions(cs));
        services.AddScoped<IAsistenciasRepository, AsistenciasRepository>();
        services.AddScoped<Modulo.Asistencias.Application.Services.AsistenciasService>();
        return services;
    }
}

public sealed record SqlOptions(string ConnectionString);
