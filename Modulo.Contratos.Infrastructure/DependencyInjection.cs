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
        services.AddScoped<ContratoCommandService>();
        services.AddScoped<IEmployeeWithoutContract, EmployeeWithoutContractRepository>();
        services.AddScoped<EmployeeWithoutContractService>();
        services.AddScoped<IContractRepository, ContractRepository>();
        services.AddScoped<ContractService>();
        services.AddScoped<IContractDetailsRepository, ContractDetailsRepository>();
        services.AddScoped<ContractDetailsService>();
        return services;
    }
}

public sealed record SqlOptions(string ConnectionString);

