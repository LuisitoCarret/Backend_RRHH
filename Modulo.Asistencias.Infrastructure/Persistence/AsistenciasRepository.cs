namespace Modulo.Asistencias.Infrastructure.Persistence;

public sealed class AsistenciasRepository : IAsistenciasRepository
{
    private readonly SqlOptions _opt;
    public AsistenciasRepository(SqlOptions opt) => _opt = opt;
    private SqlConnection NewConn() => new(_opt.ConnectionString);

    public async Task<IReadOnlyList<AsistenciaListado>> ListarAsync(DateTime? desde, DateTime? hasta, int? turnoId, CancellationToken ct)
    {
        using var cn = NewConn();
        using var cmd = new SqlCommand("asistencias.sp_asistencias_listar", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@fecha_desde", (object?)desde ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@fecha_hasta", (object?)hasta ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@turno_id", (object?)turnoId ?? DBNull.Value);

        await cn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);

        var list = new List<AsistenciaListado>();
        while (await rd.ReadAsync(ct))
        {
            list.Add(new AsistenciaListado
            {
                AsistenciaId = rd.GetInt32(rd.GetOrdinal("asistenciaId")),
                EmpleadoId = rd.GetInt32(rd.GetOrdinal("empleadoId")),
                NombreEmpleado = rd.GetString(rd.GetOrdinal("nombreEmpleado")),
                Turno = rd.GetString(rd.GetOrdinal("turno")),
                Fecha = rd.GetString(rd.GetOrdinal("fecha")),
                HoraInicioTurno = rd.GetString(rd.GetOrdinal("horaInicioTurno")),
                HoraFinTurno = rd.GetString(rd.GetOrdinal("horaFinTurno")),
                ToleranciaMinutos = rd.GetInt32(rd.GetOrdinal("toleranciaMinutos")),
                HoraEntradaReal = rd.IsDBNull(rd.GetOrdinal("horaEntradaReal")) ? null : rd.GetString(rd.GetOrdinal("horaEntradaReal")),
                HoraSalidaReal = rd.IsDBNull(rd.GetOrdinal("horaSalidaReal")) ? null : rd.GetString(rd.GetOrdinal("horaSalidaReal")),
                RetardoMinutos = rd.IsDBNull(rd.GetOrdinal("retardoMinutos")) ? (int?)null : rd.GetInt32(rd.GetOrdinal("retardoMinutos")),
                SalidaAnticipadaMinutos = rd.IsDBNull(rd.GetOrdinal("salidaAnticipadaMinutos")) ? (int?)null : rd.GetInt32(rd.GetOrdinal("salidaAnticipadaMinutos")),
                Estado = rd.GetString(rd.GetOrdinal("estado"))
            });
        }
        return list;
    }

    public async Task<AsistenciaRegistro> InsertarAsync(int empleadoId, string tipoRegistro, CancellationToken ct)
    {
        try
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("asistencias.sp_asistencia_insertar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@empleado_id", empleadoId);
            cmd.Parameters.AddWithValue("@tipo_registro", tipoRegistro);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);
            if (!await rd.ReadAsync(ct))
                throw new InvalidOperationException("Sin resultado de sp_asistencia_insertar.");

            return new AsistenciaRegistro
            {
                AsistenciaId = rd.GetInt32(rd.GetOrdinal("asistenciaId")),
                EmpleadoId = rd.GetInt32(rd.GetOrdinal("empleadoId")),
                NombreEmpleado = rd.GetString(rd.GetOrdinal("nombreEmpleado")),
                Turno = rd.GetString(rd.GetOrdinal("turno")),
                Fecha = rd.GetString(rd.GetOrdinal("fecha")),
                HoraInicioTurno = rd.GetString(rd.GetOrdinal("horaInicioTurno")),
                HoraFinTurno = rd.GetString(rd.GetOrdinal("horaFinTurno")),
                ToleranciaMinutos = rd.GetInt32(rd.GetOrdinal("toleranciaMinutos")),
                HoraEntradaReal = rd.IsDBNull(rd.GetOrdinal("horaEntradaReal")) ? null : rd.GetString(rd.GetOrdinal("horaEntradaReal")),
                HoraSalidaReal = rd.IsDBNull(rd.GetOrdinal("horaSalidaReal")) ? null : rd.GetString(rd.GetOrdinal("horaSalidaReal")),
                RetardoMinutos = rd.IsDBNull(rd.GetOrdinal("retardoMinutos")) ? (int?)null : rd.GetInt32(rd.GetOrdinal("retardoMinutos")),
                SalidaAnticipadaMinutos = rd.IsDBNull(rd.GetOrdinal("salidaAnticipadaMinutos")) ? (int?)null : rd.GetInt32(rd.GetOrdinal("salidaAnticipadaMinutos")),
                Estado = rd.GetString(rd.GetOrdinal("estado")),
                Mensaje = rd.GetString(rd.GetOrdinal("mensaje"))
            };
        }
        catch (SqlException ex) when (ex.Class >= 11) // mensajes de RAISERROR
        {
            throw new BusinessRuleException(ex.Message);
        }
    }

    public async Task<AsistenciaActualizada> ActualizarAsync(int asistenciaId, TimeSpan? horaEntrada, TimeSpan? horaSalida, string? observaciones, CancellationToken ct)
    {
        try
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("asistencias.sp_asistencia_actualizar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@asistencia_id", asistenciaId);
            cmd.Parameters.AddWithValue("@hora_entrada", (object?)horaEntrada ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@hora_salida", (object?)horaSalida ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@observaciones", (object?)observaciones ?? DBNull.Value);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);
            if (!await rd.ReadAsync(ct))
                throw new InvalidOperationException("Sin resultado de sp_asistencia_actualizar.");

            return new AsistenciaActualizada
            {
                AsistenciaId = rd.GetInt32(rd.GetOrdinal("asistenciaId")),
                EmpleadoId = rd.GetInt32(rd.GetOrdinal("empleadoId")),
                NombreEmpleado = rd.GetString(rd.GetOrdinal("nombreEmpleado")),
                Turno = rd.GetString(rd.GetOrdinal("turno")),
                Fecha = rd.GetString(rd.GetOrdinal("fecha")),
                HoraInicioTurno = rd.GetString(rd.GetOrdinal("horaInicioTurno")),
                HoraFinTurno = rd.GetString(rd.GetOrdinal("horaFinTurno")),
                ToleranciaMinutos = rd.GetInt32(rd.GetOrdinal("toleranciaMinutos")),
                HoraEntradaReal = rd.IsDBNull(rd.GetOrdinal("horaEntradaReal")) ? null : rd.GetString(rd.GetOrdinal("horaEntradaReal")),
                HoraSalidaReal = rd.IsDBNull(rd.GetOrdinal("horaSalidaReal")) ? null : rd.GetString(rd.GetOrdinal("horaSalidaReal")),
                RetardoMinutos = rd.IsDBNull(rd.GetOrdinal("retardoMinutos")) ? (int?)null : rd.GetInt32(rd.GetOrdinal("retardoMinutos")),
                Estado = rd.GetString(rd.GetOrdinal("estado")),
                Mensaje = rd.GetString(rd.GetOrdinal("mensaje"))
            };
        }
        catch (SqlException ex) when (ex.Class >= 11)
        {
            throw new BusinessRuleException(ex.Message);
        }
    }

    public async Task<IReadOnlyList<AsistenciaReporteMensual>> ReporteMensualAsync(DateTime fechaInicio, DateTime fechaFin, CancellationToken ct)
    {
        using var cn = NewConn();
        using var cmd = new SqlCommand("asistencias.sp_asistencia_reporte_mensual", cn) { CommandType = CommandType.StoredProcedure };
        cmd.Parameters.AddWithValue("@fecha_inicio", fechaInicio);
        cmd.Parameters.AddWithValue("@fecha_fin", fechaFin);

        await cn.OpenAsync(ct);
        using var rd = await cmd.ExecuteReaderAsync(ct);

        var list = new List<AsistenciaReporteMensual>();
        while (await rd.ReadAsync(ct))
        {
            list.Add(new AsistenciaReporteMensual
            {
                EmpleadoId = rd.GetInt32(rd.GetOrdinal("empleadoId")),
                NombreEmpleado = rd.GetString(rd.GetOrdinal("nombreEmpleado")),
                DiasAsistidos = rd.GetInt32(rd.GetOrdinal("diasAsistidos")),
                TotalRetardos = rd.GetInt32(rd.GetOrdinal("totalRetardos")),
                PendientesDeSalida = rd.GetInt32(rd.GetOrdinal("pendientesDeSalida")),
                TotalAusencias = rd.GetInt32(rd.GetOrdinal("totalAusencias"))
            });
        }
        return list;
    }

}
