// ==========================
// Modulo.Reclutamiento.Infrastructure/DependencyInjection.cs
// ==========================
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulo.Reclutamiento.Domain.Interfaces;
using Modulo.Reclutamiento.Infrastructure.Persistence;

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
        return services;
    }
}
