namespace Modulo.Reclutamiento.Infrastructure.Persistence
{
    public class PostulacionRepository:IPostulacionRepository
    {
        private readonly SqlOptions _opt;
        public PostulacionRepository(SqlOptions opt) => _opt = opt;

        private SqlConnection NewConn() => new(_opt.ConnectionString);

        public async Task<PostulacionDetail?> CreateAsync(PostulacionCreate data, CancellationToken ct)
        {
            return await SqlGuard.Exec(async () =>
            {
                using var cn = NewConn();
                using var cmd = new SqlCommand("reclutamiento.sp_postulacion_crear", cn)
                { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("@vacante_id", data.VacanteId);
                cmd.Parameters.AddWithValue("@nombre_contacto", data.NombreContacto);
                cmd.Parameters.AddWithValue("@email_contacto", (object?)data.EmailContacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@telefono_contacto", (object?)data.TelefonoContacto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@cv_url", (object?)data.CvUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@estatus", data.Estatus ?? "recibida");
                cmd.Parameters.AddWithValue("@observacion", (object?)data.Observacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fecha_postulacion", data.FechaPostulacion);

                await cn.OpenAsync(ct);
                using var rd = await cmd.ExecuteReaderAsync(ct);

                if (!await rd.ReadAsync(ct))
                    return null;

                return new PostulacionDetail
                {
                    PostulacionId = rd.GetInt32(rd.GetOrdinal("postulacionId")),
                    VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                    NombreContacto = rd.GetString(rd.GetOrdinal("nombreContacto")),
                    EmailContacto = rd.IsDBNull(rd.GetOrdinal("emailContacto")) ? null : rd.GetString(rd.GetOrdinal("emailContacto")),
                    TelefonoContacto = rd.IsDBNull(rd.GetOrdinal("telefonoContacto")) ? null : rd.GetString(rd.GetOrdinal("telefonoContacto")),
                    CvUrl = rd.IsDBNull(rd.GetOrdinal("cvUrl")) ? null : rd.GetString(rd.GetOrdinal("cvUrl")),
                    Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                    Observacion = rd.IsDBNull(rd.GetOrdinal("observacion")) ? null : rd.GetString(rd.GetOrdinal("observacion")),
                    FechaPostulacion = rd.GetDateTime(rd.GetOrdinal("fechaPostulacion"))
                };
            });
        }

        public async Task<PostulacionListResponse> ListAsync(
      string? vacanteNombre, string? estatus, int page, int pageSize, CancellationToken ct)
        {
            return await SqlGuard.Exec(async () =>
            {
                using var cn = NewConn();
                using var cmd = new SqlCommand("reclutamiento.sp_postulaciones_listar", cn)
                { CommandType = CommandType.StoredProcedure };

                cmd.Parameters.AddWithValue("@vacante_nombre", (object?)vacanteNombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@estatus", (object?)estatus ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page);
                cmd.Parameters.AddWithValue("@pageSize", pageSize);

                await cn.OpenAsync(ct);

                var response = new PostulacionListResponse();

                using var rd = await cmd.ExecuteReaderAsync(ct);

                // 1) RS totales
                if (await rd.ReadAsync(ct))
                {
                    response.Total = rd.GetInt32(rd.GetOrdinal("total"));
                    response.Page = rd.GetInt32(rd.GetOrdinal("page"));
                    response.PageSize = rd.GetInt32(rd.GetOrdinal("pageSize"));
                }

                // 2) RS items
                if (await rd.NextResultAsync(ct))
                {
                    while (await rd.ReadAsync(ct))
                    {
                        response.Items.Add(new PostulacionListItem
                        {
                            PostulacionId = rd.GetInt32(rd.GetOrdinal("postulacionId")),
                            VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                            NombreVacante = rd.GetString(rd.GetOrdinal("nombreVacante")),
                            NombreContacto = rd.GetString(rd.GetOrdinal("nombreContacto")),
                            Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                            FechaPostulacion = rd.GetDateTime(rd.GetOrdinal("fechaPostulacion"))
                        });
                    }
                }

                return response;
            });
        }
        public async Task<PostulacionDetails?> GetDetalleAsync(int postulacionId, CancellationToken ct)
        {
            return await SqlGuard.Exec(async () =>
            {
                using var cn = NewConn();
                using var cmd = new SqlCommand("reclutamiento.sp_postulacion_detalle", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@postulacion_id", postulacionId);

                await cn.OpenAsync(ct);
                using var rd = await cmd.ExecuteReaderAsync(ct);

                if (!await rd.ReadAsync(ct))
                    return null;

                return new PostulacionDetails
                {
                    PostulacionId = rd.GetInt32(rd.GetOrdinal("postulacionId")),
                    VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                    NombreVacante = rd.GetString(rd.GetOrdinal("nombreVacante")),
                    VacanteEstatus = rd.GetString(rd.GetOrdinal("vacanteEstatus")),
                    NombreContacto = rd.GetString(rd.GetOrdinal("nombreContacto")),
                    EmailContacto = rd.IsDBNull(rd.GetOrdinal("emailContacto")) ? null : rd.GetString(rd.GetOrdinal("emailContacto")),
                    TelefonoContacto = rd.IsDBNull(rd.GetOrdinal("telefonoContacto")) ? null : rd.GetString(rd.GetOrdinal("telefonoContacto")),
                    CvUrl = rd.IsDBNull(rd.GetOrdinal("cvUrl")) ? null : rd.GetString(rd.GetOrdinal("cvUrl")),
                    Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                    Observacion = rd.IsDBNull(rd.GetOrdinal("observacion")) ? null : rd.GetString(rd.GetOrdinal("observacion")),
                    FechaPostulacion = rd.GetDateTime(rd.GetOrdinal("fechaPostulacion"))
                };
            });
        }

        public async Task<PostulacionUpdateResult> UpdateAsync(
    int postulacionId, string estatus, string? observacion, CancellationToken ct)
        {
            return await SqlGuard.Exec(async () =>
            {
                using var cn = NewConn();
                using var cmd = new SqlCommand("reclutamiento.sp_postulacion_actualizar", cn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@postulacion_id", postulacionId);
                cmd.Parameters.AddWithValue("@estatus", estatus);
                cmd.Parameters.AddWithValue("@observacion", (object?)observacion ?? DBNull.Value);

                await cn.OpenAsync(ct);
                using var rd = await cmd.ExecuteReaderAsync(ct);

                if (!await rd.ReadAsync(ct))
                    throw new Exception("El SP no devolvió datos.");

                return new PostulacionUpdateResult
                {
                    PostulacionId = rd.GetInt32(rd.GetOrdinal("postulacionId")),
                    VacanteId = rd.GetInt32(rd.GetOrdinal("vacanteId")),
                    Estatus = rd.GetString(rd.GetOrdinal("estatus")),
                    Observacion = rd.IsDBNull(rd.GetOrdinal("observacion"))
                        ? null : rd.GetString(rd.GetOrdinal("observacion")),
                    VacanteEstatus = rd.IsDBNull(rd.GetOrdinal("vacanteEstatus"))
                        ? null : rd.GetString(rd.GetOrdinal("vacanteEstatus")),
                    VacanteFechaCierre = rd.IsDBNull(rd.GetOrdinal("vacanteFechaCierre"))
                        ? null : rd.GetDateTime(rd.GetOrdinal("vacanteFechaCierre"))
                };
            });
        }
    }
}
