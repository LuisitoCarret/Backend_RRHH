// Modulo.Evaluaciones.Infrastructure/Common/SqlGuard.cs
using Microsoft.Data.SqlClient;

namespace Modulo.Evaluaciones.Infrastructure.Common;
internal static class SqlGuard
{
    public static async Task<T> Exec<T>(Func<Task<T>> factory)
    {
        try { return await factory(); }
        catch (BusinessRuleException) { throw; }
        catch (SqlException ex) when (ex.Class >= 11)
        {
            var msg = (ex.Message ?? "").Replace("\r", " ").Replace("\n", " ").Trim();
            throw new BusinessRuleException(string.IsNullOrWhiteSpace(msg) ? "Error de negocio." : msg);
        }
    }
}
