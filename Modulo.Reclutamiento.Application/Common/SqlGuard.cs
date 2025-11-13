// ==========================
// Modulo.Reclutamiento.Infrastructure/Common/SqlGuard.cs
// ==========================
using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Modulo.Reclutamiento.Application.Common;

namespace Modulo.Reclutamiento.Infrastructure.Common;

public static class SqlGuard
{
    // Para operaciones que retornan un valor (T)
    public static async Task<T> Exec<T>(Func<Task<T>> taskFactory)
    {
        try
        {
            return await taskFactory();
        }
        catch (BusinessRuleException) { throw; } // re-lanza reglas de negocio propias
        catch (SqlException ex) when (ex.Class >= 11)
        {
            var msg = (ex.Message ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
            throw new BusinessRuleException(string.IsNullOrWhiteSpace(msg) ? "Error de negocio." : msg);
        }
    }

    // Opcional: para operaciones que no retornan valor
    public static async Task Exec(Func<Task> taskFactory)
    {
        try
        {
            await taskFactory();
        }
        catch (BusinessRuleException) { throw; }
        catch (SqlException ex) when (ex.Class >= 11)
        {
            var msg = (ex.Message ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
            throw new BusinessRuleException(string.IsNullOrWhiteSpace(msg) ? "Error de negocio." : msg);
        }
    }
}
