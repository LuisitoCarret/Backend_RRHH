using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using Microsoft.Data.SqlClient;
using Modulo.Contratos.Domain.Entities;
using Modulo.Contratos.Domain.Interfaces;

namespace Modulo.Contratos.Infrastructure.Persistence;

public sealed class ContratoRepository : IContratoRepository
{
    private readonly SqlOptions _opt;
    public ContratoRepository(SqlOptions opt) => _opt = opt;

    private SqlConnection NewConn() => new(_opt.ConnectionString);

    public async Task<Contrato> InsertAsync(Contrato data, CancellationToken ct)
    {
        using var cn = NewConn();
        using var cmd = new SqlCommand("contratos.sp_contrato_insertar", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@empleado_id", data.EmpleadoId);
        cmd.Parameters.AddWithValue("@tipo_contrato_id", data.TipoContratoId);
        cmd.Parameters.AddWithValue("@estatus_contrato_id", data.EstatusContratoId);
        cmd.Parameters.AddWithValue("@fecha_inicio", DateTime.Parse(data.FechaInicio));
        cmd.Parameters.AddWithValue("@fecha_fin", (object?)(data.FechaFin is null ? DBNull.Value : DateTime.Parse(data.FechaFin)));
        cmd.Parameters.AddWithValue("@salario_base", data.SalarioBase);
        cmd.Parameters.AddWithValue("@observaciones", (object?)data.Observaciones ?? DBNull.Value);

        await cn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct)) throw new InvalidOperationException("No se recibió resultado del SP.");

        return new Contrato
        {
            Id = rd.GetInt32(rd.GetOrdinal("id")),
            EmpleadoId = rd.GetInt32(rd.GetOrdinal("empleado_id")),
            TipoContratoId = rd.GetInt32(rd.GetOrdinal("tipo_contrato_id")),
            EstatusContratoId = rd.GetInt32(rd.GetOrdinal("estatus_contrato_id")),
            FechaInicio = rd.GetString(rd.GetOrdinal("fecha_inicio")),
            FechaFin = rd.IsDBNull(rd.GetOrdinal("fecha_fin")) ? null : rd.GetString(rd.GetOrdinal("fecha_fin")),
            SalarioBase = rd.GetDecimal(rd.GetOrdinal("salario_base")),
            Observaciones = rd.IsDBNull(rd.GetOrdinal("observaciones")) ? null : rd.GetString(rd.GetOrdinal("observaciones"))
        };
    }

    public async Task<Contrato?> UpdateAsync(int contratoId, Contrato data, CancellationToken ct)
    {
        using var cn = NewConn();
        using var cmd = new SqlCommand("contratos.sp_contrato_actualizar", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@contrato_id", contratoId);
        cmd.Parameters.AddWithValue("@tipo_contrato_id", data.TipoContratoId);
        cmd.Parameters.AddWithValue("@estatus_contrato_id", data.EstatusContratoId);
        cmd.Parameters.AddWithValue("@fecha_inicio", DateTime.Parse(data.FechaInicio));
        cmd.Parameters.AddWithValue("@fecha_fin", (object?)(data.FechaFin is null ? DBNull.Value : DateTime.Parse(data.FechaFin)));
        cmd.Parameters.AddWithValue("@salario_base", data.SalarioBase);
        cmd.Parameters.AddWithValue("@observaciones", (object?)data.Observaciones ?? DBNull.Value);

        await cn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct)) return null;

        return new Contrato
        {
            Id = rd.GetInt32(rd.GetOrdinal("id")),
            EmpleadoId = rd.GetInt32(rd.GetOrdinal("empleado_id")),
            TipoContratoId = rd.GetInt32(rd.GetOrdinal("tipo_contrato_id")),
            EstatusContratoId = rd.GetInt32(rd.GetOrdinal("estatus_contrato_id")),
            FechaInicio = rd.GetString(rd.GetOrdinal("fecha_inicio")),
            FechaFin = rd.IsDBNull(rd.GetOrdinal("fecha_fin")) ? null : rd.GetString(rd.GetOrdinal("fecha_fin")),
            SalarioBase = rd.GetDecimal(rd.GetOrdinal("salario_base")),
            Observaciones = rd.IsDBNull(rd.GetOrdinal("observaciones")) ? null : rd.GetString(rd.GetOrdinal("observaciones"))
        };
    }

    public async Task<bool> DeleteAsync(int contratoId, CancellationToken ct)
    {
        using var cn = NewConn();
        using var cmd = new SqlCommand("contratos.sp_contrato_eliminar", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@contrato_id", contratoId);

        await cn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct)) return false;
        return rd.GetInt32(rd.GetOrdinal("ok")) == 1;
    }

    public async Task<RenovacionContrato> RenewAsync(int contratoId, DateTime fechaRenovacion, DateTime nuevaFechaFin, string? comentario, CancellationToken ct)
    {
        using var cn = NewConn();
        using var cmd = new SqlCommand("contratos.sp_contrato_renovar", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@contrato_id", contratoId);
        cmd.Parameters.AddWithValue("@fecha_renovacion", fechaRenovacion);
        cmd.Parameters.AddWithValue("@nueva_fecha_fin", nuevaFechaFin);
        cmd.Parameters.AddWithValue("@comentario", (object?)comentario ?? DBNull.Value);

        await cn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);
        if (!await rd.ReadAsync(ct)) throw new InvalidOperationException("No se recibió resultado de renovación.");

        return new RenovacionContrato
        {
            RenovacionId = rd.GetInt32(rd.GetOrdinal("renovacion_id")),
            ContratoId = rd.GetInt32(rd.GetOrdinal("contrato_id")),
            FechaRenovacion = rd.GetString(rd.GetOrdinal("fecha_renovacion")),
            NuevaFechaFin = rd.GetString(rd.GetOrdinal("nueva_fecha_fin")),
            Comentario = rd.IsDBNull(rd.GetOrdinal("comentario")) ? null : rd.GetString(rd.GetOrdinal("comentario"))
        };
    }
}
