using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulo.Contratos.Domain.Interfaces;
using Modulo.Contratos.Infrastructure.Persistence;

namespace Modulo.Contratos.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddContratosModule(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("DefaultConnection")
                 ?? cfg["SqlServer:ConnectionString"]
                 ?? throw new InvalidOperationException("Connection string not found.");

        services.AddSingleton(new SqlOptions(cs));
        services.AddScoped<IContratoRepository, ContratoRepository>();
        return services;
    }
}

public sealed record SqlOptions(string ConnectionString);

