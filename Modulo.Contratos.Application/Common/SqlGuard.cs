using Microsoft.Data.SqlClient;
using System.Text;
using Modulo.Contratos.Application.Common;

namespace Modulo.Contratos.Infrastructure.Common;

public static class SqlGuard
{
    /// Envuelve llamadas al SQL Server. Si un SP hace RAISERROR (severidad >= 11),
    /// convertimos a BusinessRuleException con el mensaje limpio.
    public static async Task<T> Exec<T>(Func<Task<T>> dbCall)
    {
        try { return await dbCall(); }
        catch (SqlException ex) when (ex.Class >= 11) // errores de negocio desde RAISERROR
        {
            throw new BusinessRuleException(Cleanup(ex.Message));
        }
    }

    private static string Cleanup(string msg)
    {
        if (string.IsNullOrWhiteSpace(msg)) return "Error de negocio.";
        var s = msg.Replace("\r", "").Replace("\n", " ").Trim();
        while (s.Contains("  ")) s = s.Replace("  ", " ");
        return s;
    }
}
