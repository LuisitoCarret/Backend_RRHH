// Modulo.Evaluaciones.Infrastructure/SqlOptions.cs
namespace Modulo.Evaluaciones.Infrastructure;
public sealed class SqlOptions
{
    public string ConnectionString { get; }
    public SqlOptions(string cs) => ConnectionString = cs;
}
