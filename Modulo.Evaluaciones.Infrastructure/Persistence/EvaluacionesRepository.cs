// ==========================
// Modulo.Evaluaciones.Infrastructure/Persistence/EvaluacionesRepository.cs
// ==========================
using System.Data;
using Microsoft.Data.SqlClient;
using Modulo.Evaluaciones.Domain.Entities;
using Modulo.Evaluaciones.Domain.Interfaces;
using Modulo.Evaluaciones.Infrastructure.Common;

namespace Modulo.Evaluaciones.Infrastructure.Persistence;

public sealed class EvaluacionesRepository : IEvaluacionesRepository
{
    private readonly SqlOptions _opt;
    public EvaluacionesRepository(SqlOptions opt) => _opt = opt;

    private SqlConnection NewConn() => new(_opt.ConnectionString);

    // ---------------------------------------------------------------------
    // INDICADORES (Catálogo)
    // ---------------------------------------------------------------------
    public async Task<IReadOnlyList<CatalogoIndicador>> ListCatalogoAsync(CancellationToken ct)
        => await SqlGuard.Exec(async () =>
        {
            var list = new List<CatalogoIndicador>();
            using var cn = NewConn();
            using var cmd = new SqlCommand("evaluaciones.sp_indicadores_catalogo_listar", cn)
            { CommandType = CommandType.StoredProcedure };

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);
            while (await rd.ReadAsync(ct))
            {
                list.Add(new CatalogoIndicador
                {
                    CatalogoId = rd.GetInt32(rd.GetOrdinal("catalogo_id")),
                    Nombre = rd.GetString(rd.GetOrdinal("nombre")),
                    Descripcion = rd.IsDBNull(rd.GetOrdinal("descripcion"))
                        ? null : rd.GetString(rd.GetOrdinal("descripcion"))
                });
            }
            return (IReadOnlyList<CatalogoIndicador>)list;
        });

    // ---------------------------------------------------------------------
    // PLANTILLAS (listar / detalle / crear / vigencia)
    // ---------------------------------------------------------------------
    public async Task<IReadOnlyList<PlantillaListItem>> ListPlantillasAsync(int? areaId, bool? vigente, CancellationToken ct)
        => await SqlGuard.Exec(async () =>
        {
            var list = new List<PlantillaListItem>();
            using var cn = NewConn();
            using var cmd = new SqlCommand("evaluaciones.sp_plantilla_listar", cn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@area_id", (object?)areaId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@vigente", (object?)vigente ?? DBNull.Value);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);
            while (await rd.ReadAsync(ct))
            {
                list.Add(new PlantillaListItem
                {
                    PlantillaId = rd.GetInt32(rd.GetOrdinal("plantilla_id")),
                    Nombre = rd.GetString(rd.GetOrdinal("nombre")),
                    AreaId = rd.GetInt32(rd.GetOrdinal("area_id")),
                    NombreArea = rd.GetString(rd.GetOrdinal("nombre_area")),
                    PeriodoInicio = rd.GetDateTime(rd.GetOrdinal("periodo_inicio")),
                    PeriodoFin = rd.GetDateTime(rd.GetOrdinal("periodo_fin")),
                    Vigente = rd.GetBoolean(rd.GetOrdinal("vigente"))
                });
            }
            return (IReadOnlyList<PlantillaListItem>)list;
        });

    public async Task<PlantillaDetalle?> GetPlantillaDetalleAsync(int plantillaId, CancellationToken ct)
        => await SqlGuard.Exec(async () =>
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("evaluaciones.sp_plantilla_detalle", cn)
            { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@plantilla_id", plantillaId);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            if (!await rd.ReadAsync(ct)) return null;

            var d = new PlantillaDetalle
            {
                PlantillaId = rd.GetInt32(rd.GetOrdinal("plantilla_id")),
                Nombre = rd.GetString(rd.GetOrdinal("nombre")),
                Descripcion = rd.IsDBNull(rd.GetOrdinal("descripcion"))
                    ? null : rd.GetString(rd.GetOrdinal("descripcion")),
                AreaId = rd.GetInt32(rd.GetOrdinal("area_id")),
                NombreArea = rd.GetString(rd.GetOrdinal("nombre_area")),
                PeriodoInicio = rd.GetDateTime(rd.GetOrdinal("periodo_inicio")),
                PeriodoFin = rd.GetDateTime(rd.GetOrdinal("periodo_fin")),
                Vigente = rd.GetBoolean(rd.GetOrdinal("vigente"))
            };

            if (await rd.NextResultAsync(ct))
            {
                while (await rd.ReadAsync(ct))
                {
                    d.Indicadores.Add(new PlantillaIndicadorDetalle
                    {
                        IndicadorId = rd.GetInt32(rd.GetOrdinal("indicador_id")),
                        CatalogoId = rd.GetInt32(rd.GetOrdinal("catalogo_id")),
                        NombreIndicador = rd.GetString(rd.GetOrdinal("nombre_indicador")),
                        Ponderacion = rd.GetDecimal(rd.GetOrdinal("ponderacion"))
                    });
                }
            }

            return d;
        });

    public async Task<(int plantillaId, string mensaje)> CrearPlantillaAsync( string nombre, string? descripcion, int areaId, DateTime periodoInicio, DateTime periodoFin, string indicadoresJson, CancellationToken ct)
        => await SqlGuard.Exec(async () =>
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("evaluaciones.sp_plantilla_crear", cn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@nombre", nombre);
            cmd.Parameters.AddWithValue("@descripcion", (object?)descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@area_id", areaId);
            cmd.Parameters.AddWithValue("@periodo_inicio", periodoInicio);
            cmd.Parameters.AddWithValue("@periodo_fin", periodoFin);
            cmd.Parameters.Add("@indicadores", SqlDbType.NVarChar, -1).Value = indicadoresJson;

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);
            if (!await rd.ReadAsync(ct)) throw new InvalidOperationException("SP no devolvió resultado.");

            return (rd.GetInt32(rd.GetOrdinal("plantilla_id")),
                    rd.GetString(rd.GetOrdinal("mensaje")));
        });

    public async Task<string> ActualizarVigenciaAsync(int plantillaId, bool vigente, CancellationToken ct)
        => await SqlGuard.Exec(async () =>
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("evaluaciones.sp_plantilla_actualizar_vigencia", cn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@plantilla_id", plantillaId);
            cmd.Parameters.AddWithValue("@vigente", vigente);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);
            if (!await rd.ReadAsync(ct)) throw new InvalidOperationException("SP no devolvió resultado.");
            return rd.GetString(rd.GetOrdinal("mensaje"));
        });

    // ---------------------------------------------------------------------
    // EVALUACIONES (crear / actualizar / listar / detalle / por empleado)
    // ---------------------------------------------------------------------
    public async Task<(int evaluacionId, string estatus, string mensaje)>
        CrearEvaluacionAsync(int empleadoId, int plantillaId)
        => await SqlGuard.Exec(async () =>
        {
            using var connection = NewConn();
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_crear", connection)
            { CommandType = CommandType.StoredProcedure };

            command.Parameters.AddWithValue("@empleado_id", empleadoId);
            command.Parameters.AddWithValue("@plantilla_id", plantillaId);

            await connection.OpenAsync();

            int evaluacionId = 0;
            string estatus = "";
            string mensaje = "";

            using (var reader = await command.ExecuteReaderAsync())
            {
                // Avanza resultsets vacíos
                while (!reader.HasRows && await reader.NextResultAsync()) { }

                if (reader.HasRows && await reader.ReadAsync())
                {
                    evaluacionId = reader["evaluacion_id"] != DBNull.Value
                        ? Convert.ToInt32(reader["evaluacion_id"])
                        : 0;

                    estatus = reader["estatus"]?.ToString() ?? "";
                    mensaje = reader["mensaje"]?.ToString() ?? "";
                }
            }

            return (evaluacionId, estatus, mensaje);
        });

    public async Task<(int evaluacionId, decimal? puntajeTotal, string? nivelDesempeno, string estatus, string mensaje)>
        ActualizarEvaluacionAsync(int evaluacionId, string detalleJson, string retro, string estatusNuevo)
        => await SqlGuard.Exec(async () =>
        {
            using var connection = NewConn();
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_actualizar", connection)
            { CommandType = CommandType.StoredProcedure };

            command.Parameters.AddWithValue("@evaluacion_id", evaluacionId);
            command.Parameters.AddWithValue("@detalle", detalleJson);
            command.Parameters.AddWithValue("@retroalimentacion", (object?)retro ?? DBNull.Value);
            command.Parameters.AddWithValue("@estatus", estatusNuevo);

            await connection.OpenAsync();

            int evalId = 0;
            decimal? puntaje = null;
            string? nivel = null;
            string newStatus = "";
            string mensaje = "";

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                evalId = reader["evaluacion_id"] != DBNull.Value
                    ? Convert.ToInt32(reader["evaluacion_id"]) : 0;

                // Si viene cierre, el SP devuelve más columnas
                if (reader.FieldCount > 3)
                {
                    puntaje = reader["puntaje_total"] != DBNull.Value ? Convert.ToDecimal(reader["puntaje_total"]) : null;
                    nivel = reader["nivel_desempeno"]?.ToString();
                    newStatus = reader["estatus"].ToString()!;
                }
                else
                {
                    newStatus = reader["estatus"].ToString()!;
                    mensaje = reader["mensaje"].ToString()!;
                }
            }

            return (evalId, puntaje, nivel, newStatus, mensaje);
        });

    public async Task<IEnumerable<ListarEvaluacionResult>> ListarEvaluacionesAsync(int? areaId, string? estatus)
        => await SqlGuard.Exec(async () =>
        {
            var result = new List<ListarEvaluacionResult>();

            using var connection = NewConn();
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_listar", connection)
            { CommandType = CommandType.StoredProcedure };

            command.Parameters.AddWithValue("@area_id", (object?)areaId ?? DBNull.Value);
            command.Parameters.AddWithValue("@estatus", (object?)estatus ?? DBNull.Value);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new ListarEvaluacionResult
                {
                    EvaluacionId = Convert.ToInt32(reader["evaluacion_id"]),
                    EmpleadoId = Convert.ToInt32(reader["empleado_id"]),
                    NombreEmpleado = reader["nombre_empleado"].ToString()!,
                    PlantillaId = Convert.ToInt32(reader["plantilla_id"]),
                    NombrePlantilla = reader["nombre_plantilla"].ToString()!,
                    AreaId = Convert.ToInt32(reader["area_id"]),
                    NombreArea = reader["nombre_area"].ToString()!,
                    PuntajeTotal = reader["puntaje_total"] as decimal?,
                    NivelDesempeno = reader["nivel_desempeno"]?.ToString(),
                    Estatus = reader["estatus"].ToString()!
                });
            }

            return result;
        });

    public async Task<DetalleEvaluacionResult?> ObtenerDetalleEvaluacionAsync(int evaluacionId)
        => await SqlGuard.Exec(async () =>
        {
            using var connection = NewConn();
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_detalle", connection)
            { CommandType = CommandType.StoredProcedure };

            command.Parameters.AddWithValue("@evaluacion_id", evaluacionId);

            await connection.OpenAsync();

            // 1) Cabecera
            using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            var result = new DetalleEvaluacionResult
            {
                EvaluacionId = Convert.ToInt32(reader["evaluacion_id"]),
                Empleado = reader["empleado"].ToString()!,
                AreaId = Convert.ToInt32(reader["area_id"]),
                NombreArea = reader["nombre_area"].ToString()!,
                Plantilla = reader["plantilla"].ToString()!,
                PuntajeTotal = reader["puntaje_total"] as decimal?,
                NivelDesempeno = reader["nivel_desempeno"]?.ToString(),
                Retroalimentacion = reader["retroalimentacion"]?.ToString(),
                Estatus = reader["estatus"].ToString()!
            };

            // 2) Detalle
            await reader.NextResultAsync();

            var detalleList = new List<DetalleIndicadorItem>();
            while (await reader.ReadAsync())
            {
                detalleList.Add(new DetalleIndicadorItem
                {
                    Indicador = reader["indicador"].ToString()!,
                    Ponderacion = Convert.ToDecimal(reader["ponderacion"]),
                    Calificacion = reader["calificacion"] as decimal?
                });
            }

            result.Detalle = detalleList;
            return result;
        });

    public async Task<IEnumerable<ListarEvaluacionResult>> ListarPorEmpleadoAsync(int empleadoId)
        => await SqlGuard.Exec(async () =>
        {
            var result = new List<ListarEvaluacionResult>();

            using var connection = NewConn();
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_listar_por_empleado", connection)
            { CommandType = CommandType.StoredProcedure };

            command.Parameters.AddWithValue("@empleado_id", empleadoId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new ListarEvaluacionResult
                {
                    EvaluacionId = Convert.ToInt32(reader["evaluacion_id"]),
                    EmpleadoId = Convert.ToInt32(reader["empleado_id"]),
                    NombreEmpleado = reader["nombre_empleado"].ToString()!,
                    PlantillaId = Convert.ToInt32(reader["plantilla_id"]),
                    NombrePlantilla = reader["nombre_plantilla"].ToString()!,
                    AreaId = Convert.ToInt32(reader["area_id"]),
                    NombreArea = reader["nombre_area"].ToString()!,
                    PuntajeTotal = reader["puntaje_total"] as decimal?,
                    NivelDesempeno = reader["nivel_desempeno"]?.ToString(),
                    Estatus = reader["estatus"].ToString()!
                });
            }

            return result;
        });
}
