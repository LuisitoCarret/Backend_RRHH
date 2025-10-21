using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulo.Empleados.Application.Services;
using Modulo.Empleados.Domain.Interfaces;
using Modulo.Empleados.Infrastructure.Persistence;

namespace Modulo.Empleados.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddEmpleadosModule(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("DefaultConnection")
                     ?? config["SqlServer:ConnectionString"]
                     ?? throw new InvalidOperationException("Connection string not found.");
        services.AddSingleton(new SqlOptions(connStr));
        services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
        services.AddScoped<EmpleadoQueryService>();
        return services;
    }
}

public sealed record SqlOptions(string ConnectionString);
