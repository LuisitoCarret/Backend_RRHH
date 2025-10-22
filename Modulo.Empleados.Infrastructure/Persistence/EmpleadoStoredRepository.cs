namespace Modulo.Empleados.Infrastructure.Persistence
{
    public class EmpleadoStoredRepository:IEmpleadoStoredRepository
    {
        private readonly string _connectionString;

        public EmpleadoStoredRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        private SqlConnection NewConn() => new SqlConnection(_connectionString);

        // 🔹 Obtener todos los empleados
        public async Task<List<Empleado>> GetAllAsync(CancellationToken ct)
        {
            using var conn = NewConn();
            using var cmd = new SqlCommand("empleados.sp_empleados_listar", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            var list = new List<Empleado>();
            while (await rd.ReadAsync(ct))
            {
                list.Add(new Empleado
                {
                    Id = rd.GetInt32(rd.GetOrdinal("id")),
                    Nombre = rd.GetString(rd.GetOrdinal("nombre")),
                    Email = rd.GetString(rd.GetOrdinal("correo")),
                    Telefono = rd.GetString(rd.GetOrdinal("telefono")),
                    FechaIngreso = DateOnly.Parse(rd.GetString(rd.GetOrdinal("fechaIngreso"))),
                    Area = new Area { Nombre = rd.GetString(rd.GetOrdinal("area")) },
                    Puesto = new Puesto { Nombre = rd.GetString(rd.GetOrdinal("puesto")) },
                    Turno = new Turno { Nombre = rd.GetString(rd.GetOrdinal("turno")) },
                    Estatus = new EstatusEmpleado { Nombre = rd.GetString(rd.GetOrdinal("estatus")) }
                });
            }

            return list;
        }

        // 🔹 Obtener detalle de empleado
        public async Task<Empleado?> GetByIdAsync(int id, CancellationToken ct)
        {
            using var conn = NewConn();
            using var cmd = new SqlCommand("empleados.sp_empleados_detalle", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);

            await conn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            if (!await rd.ReadAsync(ct)) return null;

            return new Empleado
            {
                Id = rd.GetInt32(rd.GetOrdinal("id")),
                Nombre = rd.GetString(rd.GetOrdinal("nombre")),
                Email = rd.GetString(rd.GetOrdinal("correo")),
                Telefono = rd.GetString(rd.GetOrdinal("telefono")),
                FechaIngreso = DateOnly.Parse(rd.GetString(rd.GetOrdinal("fechaIngreso"))),
                Area = new Area { Nombre = rd.GetString(rd.GetOrdinal("area")) },
                Puesto = new Puesto { Nombre = rd.GetString(rd.GetOrdinal("puesto")) },
                Turno = new Turno { Nombre = rd.GetString(rd.GetOrdinal("turno")) },
                Estatus = new EstatusEmpleado { Nombre = rd.GetString(rd.GetOrdinal("estatus")) },
                Domicilio = rd.IsDBNull(rd.GetOrdinal("calle")) ? null : new DomicilioEmpleado
                {
                    Calle = rd.GetString(rd.GetOrdinal("calle")),
                    Numero = rd.GetString(rd.GetOrdinal("numero")),
                    Colonia = rd.GetString(rd.GetOrdinal("colonia")),
                    Ciudad = rd.GetString(rd.GetOrdinal("ciudad")),
                    Estado = rd.GetString(rd.GetOrdinal("estado")),
                    CodigoPostal = rd.GetString(rd.GetOrdinal("codigoPostal"))
                },
                ContactoEmergencia = rd.IsDBNull(rd.GetOrdinal("contactoNombre")) ? null : new ContactoEmergencia
                {
                    Nombre = rd.GetString(rd.GetOrdinal("contactoNombre")),
                    Parentesco = rd.GetString(rd.GetOrdinal("parentesco")),
                    Telefono = rd.GetString(rd.GetOrdinal("contactoTelefono"))
                }
            };
        }

        // 🔹 Perfil del empleado autenticado
        public async Task<(DomicilioEmpleado?, ContactoEmergencia?)?> GetMeAsync(int empleadoId, CancellationToken ct)
        {
            using var conn = NewConn();
            using var cmd = new SqlCommand("empleados.sp_empleado_me", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empleado_id", empleadoId);

            await conn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            if (!await rd.ReadAsync(ct)) return null;

            var domicilio = rd.IsDBNull(rd.GetOrdinal("calle")) ? null : new DomicilioEmpleado
            {
                Calle = rd.GetString(rd.GetOrdinal("calle")),
                Numero = rd.GetString(rd.GetOrdinal("numero")),
                Colonia = rd.GetString(rd.GetOrdinal("colonia")),
                Ciudad = rd.GetString(rd.GetOrdinal("ciudad")),
                Estado = rd.GetString(rd.GetOrdinal("estado")),
                CodigoPostal = rd.GetString(rd.GetOrdinal("codigoPostal"))
            };

            var contacto = rd.IsDBNull(rd.GetOrdinal("contactoNombre")) ? null : new ContactoEmergencia
            {
                Nombre = rd.GetString(rd.GetOrdinal("contactoNombre")),
                Parentesco = rd.GetString(rd.GetOrdinal("parentesco")),
                Telefono = rd.GetString(rd.GetOrdinal("contactoTelefono"))
            };

            return (domicilio, contacto);
        }

        // 🔹 Actualizar perfil del empleado autenticado
        public async Task<(DomicilioEmpleado?, ContactoEmergencia?)?> UpdateMeAsync(
            int empleadoId,
            DomicilioEmpleado domicilio,
            ContactoEmergencia contacto,
            CancellationToken ct)
        {
            using var conn = NewConn();
            using var cmd = new SqlCommand("empleados.sp_empleado_me_update", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@empleado_id", empleadoId);
            cmd.Parameters.AddWithValue("@calle", domicilio.Calle);
            cmd.Parameters.AddWithValue("@numero", domicilio.Numero);
            cmd.Parameters.AddWithValue("@colonia", domicilio.Colonia);
            cmd.Parameters.AddWithValue("@ciudad", domicilio.Ciudad);
            cmd.Parameters.AddWithValue("@estado", domicilio.Estado);
            cmd.Parameters.AddWithValue("@codigoPostal", domicilio.CodigoPostal);
            cmd.Parameters.AddWithValue("@contactoNombre", contacto.Nombre);
            cmd.Parameters.AddWithValue("@parentesco", contacto.Parentesco);
            cmd.Parameters.AddWithValue("@contactoTel", contacto.Telefono);

            await conn.OpenAsync(ct);
            using var rd = await cmd.ExecuteReaderAsync(ct);

            if (!await rd.ReadAsync(ct)) return null;

            var dom = rd.IsDBNull(rd.GetOrdinal("calle")) ? null : new DomicilioEmpleado
            {
                Calle = rd.GetString(rd.GetOrdinal("calle")),
                Numero = rd.GetString(rd.GetOrdinal("numero")),
                Colonia = rd.GetString(rd.GetOrdinal("colonia")),
                Ciudad = rd.GetString(rd.GetOrdinal("ciudad")),
                Estado = rd.GetString(rd.GetOrdinal("estado")),
                CodigoPostal = rd.GetString(rd.GetOrdinal("codigoPostal"))
            };

            var con = rd.IsDBNull(rd.GetOrdinal("contactoNombre")) ? null : new ContactoEmergencia
            {
                Nombre = rd.GetString(rd.GetOrdinal("contactoNombre")),
                Parentesco = rd.GetString(rd.GetOrdinal("parentesco")),
                Telefono = rd.GetString(rd.GetOrdinal("contactoTelefono"))
            };

            return (dom, con);
        }
    }
}
