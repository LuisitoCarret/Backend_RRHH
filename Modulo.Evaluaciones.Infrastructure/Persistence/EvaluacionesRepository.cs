namespace Modulo.Evaluaciones.Infrastructure.Persistence
{
    public class EvaluacionesRepository:IEvaluacionesRepository
    {
        private readonly string _connectionString;

        public EvaluacionesRepository(SqlOptions options)
        {
            _connectionString = options.ConnectionString;
        }


        public async Task<(int evaluacionId, string estatus, string mensaje)>
    CrearEvaluacionAsync(int empleadoId, int plantillaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_crear", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@empleado_id", empleadoId);
            command.Parameters.AddWithValue("@plantilla_id", plantillaId);

            await connection.OpenAsync();

            int evaluacionId = 0;
            string estatus = "";
            string mensaje = "";

            using (var reader = await command.ExecuteReaderAsync())
            {
                // IGNORA todos los resultsets vacíos
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
        }


        public async Task<(int evaluacionId, decimal? puntajeTotal, string? nivelDesempeno, string estatus, string mensaje)>
            ActualizarEvaluacionAsync(int evaluacionId, string detalleJson, string retro, string estatus)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_actualizar", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@evaluacion_id", evaluacionId);
            command.Parameters.AddWithValue("@detalle", detalleJson);
            command.Parameters.AddWithValue("@retroalimentacion", retro ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@estatus", estatus);

            await connection.OpenAsync();

            int evalId = 0;
            decimal? puntaje = null;
            string? nivel = null;
            string newStatus = "";
            string mensaje = "";

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                evalId = reader["evaluacion_id"] != DBNull.Value ? Convert.ToInt32(reader["evaluacion_id"]) : 0;

                if (reader.FieldCount > 3) // cierre
                {
                    puntaje = reader["puntaje_total"] != DBNull.Value ? Convert.ToDecimal(reader["puntaje_total"]) : null;
                    nivel = reader["nivel_desempeno"]?.ToString();
                    newStatus = reader["estatus"].ToString()!;
                }
                else // borrador
                {
                    newStatus = reader["estatus"].ToString()!;
                    mensaje = reader["mensaje"].ToString()!;
                }
            }

            return (evalId, puntaje, nivel, newStatus, mensaje);
        }

        public async Task<IEnumerable<ListarEvaluacionResult>> ListarEvaluacionesAsync(int? areaId, string? estatus)
        {
            var result = new List<ListarEvaluacionResult>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_listar", connection);
            command.CommandType = CommandType.StoredProcedure;

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
        }

        public async Task<DetalleEvaluacionResult?> ObtenerDetalleEvaluacionAsync(int evaluacionId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_detalle", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@evaluacion_id", evaluacionId);

            await connection.OpenAsync();

            // 1. Leer cabecera
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
                Estatus = reader["estatus"].ToString()!,
            };

            // 2. Leer segundo resultset (detalle)
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
        }

        public async Task<IEnumerable<ListarEvaluacionResult>> ListarPorEmpleadoAsync(int empleadoId)
        {
            var result = new List<ListarEvaluacionResult>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("evaluaciones.sp_evaluacion_listar_por_empleado", connection);
            command.CommandType = CommandType.StoredProcedure;

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
        }
    }
}
