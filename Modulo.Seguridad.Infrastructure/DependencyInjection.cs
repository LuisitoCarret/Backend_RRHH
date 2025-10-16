using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulo.Seguridad.Domain.Interfaces;
using Modulo.Seguridad.Infrastructure.Persistence;

namespace Modulo.Seguridad.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSeguridadInfrastructure(
            this IServiceCollection services,
            IConfiguration config)
        {
            var cs = config.GetConnectionString("DefaultConnection")
                     ?? throw new InvalidOperationException("DefaultConnection not found.");

            services.AddSingleton<IUsuarioRepository>(new UsuarioRepository(cs));
            return services;
        }
    }
}
