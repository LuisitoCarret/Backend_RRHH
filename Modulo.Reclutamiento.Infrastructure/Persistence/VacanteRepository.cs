using Modulo.Reclutamiento.Domain.Entities;

namespace Modulo.Reclutamiento.Infrastructure.Persistence;

public sealed class VacanteRepository : IVacanteRepository
{
    private readonly SqlOptions _opt;
    public VacanteRepository(SqlOptions opt) => _opt = opt;

    private SqlConnection NewConn() => new(_opt.ConnectionString);

    public async Task<VacanteDetail> CreateAsync(VacanteCreate data, CancellationToken ct)
    {
        return await SqlGuard.Exec(async () =>
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("reclutamiento.sp_vacante_crear", cn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@titulo", data.Titulo);
            cmd.Parameters.AddWithValue("@descripcion", (object?)data.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@area_id", data.AreaId);
            cmd.Parameters.AddWithValue("@puesto_id", data.PuestoId);
            cmd.Parameters.AddWithValue("@fecha_publicacion", data.FechaPublicacion);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);
            if (!await rd.ReadAsync(ct))
                throw new InvalidOperationException("SP no devolvió datos.");

            return new VacanteDetail
            {
                VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                Titulo = rd.GetString(rd.GetOrdinal("titulo")),
                Descripcion = rd.IsDBNull(rd.GetOrdinal("descripcion")) ? null : rd.GetString(rd.GetOrdinal("descripcion")),
                AreaId = rd.GetInt32(rd.GetOrdinal("areaId")),
                NombreArea = rd.GetString(rd.GetOrdinal("nombreArea")),
                PuestoId = rd.GetInt32(rd.GetOrdinal("puestoId")),
                NombrePuesto = rd.GetString(rd.GetOrdinal("nombrePuesto")),
                Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                FechaPublicacion = rd.GetDateTime(rd.GetOrdinal("fechaPublicacion")),
                FechaCierre = rd.IsDBNull(rd.GetOrdinal("fechaCierre")) ? null : rd.GetDateTime(rd.GetOrdinal("fechaCierre"))
            };
        });
    }

    public async Task<VacanteListResponse> ListAsync(string? estatus, int? areaId, int? puestoId, int page, int pageSize, CancellationToken ct)
    {
        return await SqlGuard.Exec(async () =>
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("reclutamiento.sp_vacantes_listar", cn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@estatus", (object?)estatus ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@area_id", (object?)areaId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@puesto_id", (object?)puestoId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@page", page);
            cmd.Parameters.AddWithValue("@pageSize", pageSize);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            var response = new VacanteListResponse();

            // RS1: totales
            if (await rd.ReadAsync(ct))
            {
                response.Total = rd.GetInt32(rd.GetOrdinal("total"));
                response.Page = rd.GetInt32(rd.GetOrdinal("page"));
                response.PageSize = rd.GetInt32(rd.GetOrdinal("pageSize"));
            }

            // RS2: items
            if (await rd.NextResultAsync(ct))
            {
                while (await rd.ReadAsync(ct))
                {
                    response.Items.Add(new VacanteItem
                    {
                        VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                        Titulo = rd.GetString(rd.GetOrdinal("titulo")),
                        AreaId = rd.GetInt32(rd.GetOrdinal("areaId")),
                        NombreArea = rd.GetString(rd.GetOrdinal("nombreArea")),
                        PuestoId = rd.GetInt32(rd.GetOrdinal("puestoId")),
                        NombrePuesto = rd.GetString(rd.GetOrdinal("nombrePuesto")),
                        Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                        FechaPublicacion = rd.GetDateTime(rd.GetOrdinal("fechaPublicacion"))
                    });
                }
            }

            return response;
        });
    }

    public async Task<VacanteDetail?> GetDetalleAsync(int vacanteId, CancellationToken ct)
    {
        return await SqlGuard.Exec(async () =>
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("reclutamiento.sp_vacante_detalle", cn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@vacante_id", vacanteId);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            if (!await rd.ReadAsync(ct))
                return null;

            return new VacanteDetail
            {
                VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                Titulo = rd.GetString(rd.GetOrdinal("titulo")),
                Descripcion = rd.IsDBNull(rd.GetOrdinal("descripcion")) ? null : rd.GetString(rd.GetOrdinal("descripcion")),
                AreaId = rd.GetInt32(rd.GetOrdinal("areaId")),
                NombreArea = rd.GetString(rd.GetOrdinal("nombreArea")),
                PuestoId = rd.GetInt32(rd.GetOrdinal("puestoId")),
                NombrePuesto = rd.GetString(rd.GetOrdinal("nombrePuesto")),
                Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                FechaPublicacion = rd.GetDateTime(rd.GetOrdinal("fechaPublicacion")),
                FechaCierre = rd.IsDBNull(rd.GetOrdinal("fechaCierre")) ? null : rd.GetDateTime(rd.GetOrdinal("fechaCierre"))
            };
        });
    }

    public async Task<VacanteDetail?> UpdateAsync(VacanteUpdate data, CancellationToken ct)
    {
        return await SqlGuard.Exec(async () =>
        {
            using var cn = NewConn();
            using var cmd = new SqlCommand("reclutamiento.sp_vacante_actualizar", cn)
            { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@vacante_id", data.VacanteId);
            cmd.Parameters.AddWithValue("@titulo", (object?)data.Titulo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@descripcion", (object?)data.Descripcion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@area_id", (object?)data.AreaId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@puesto_id", (object?)data.PuestoId ?? DBNull.Value);

            await cn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            if (!await rd.ReadAsync(ct))
                return null;

            return new VacanteDetail
            {
                VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                Titulo = rd.GetString(rd.GetOrdinal("titulo")),
                Descripcion = rd.IsDBNull(rd.GetOrdinal("descripcion")) ? null : rd.GetString(rd.GetOrdinal("descripcion")),
                AreaId = rd.GetInt32(rd.GetOrdinal("areaId")),
                NombreArea = rd.GetString(rd.GetOrdinal("nombreArea")),
                PuestoId = rd.GetInt32(rd.GetOrdinal("puestoId")),
                NombrePuesto = rd.GetString(rd.GetOrdinal("nombrePuesto")),
                Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                FechaPublicacion = rd.GetDateTime(rd.GetOrdinal("fechaPublicacion")),
                FechaCierre = rd.IsDBNull(rd.GetOrdinal("fechaCierre")) ? null : rd.GetDateTime(rd.GetOrdinal("fechaCierre"))
            };
        });
    }
}
