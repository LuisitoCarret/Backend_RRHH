namespace Modulo.Empleados.Infrastructure.Persistence
{
    public class CatalogoRepository: ICatalogoRepository
    {
        private readonly string _connectionString;

        public CatalogoRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<dynamic>> GetAreasAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync("empleados.sp_area_listar", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<dynamic>> GetPuestosAsync(long? areaId = null)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new { area_id = areaId };
            return await connection.QueryAsync("empleados.sp_puesto_listar", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<dynamic>> GetTurnosAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync("empleados.sp_turno_listar", commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<dynamic>> GetEstatusAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync("empleados.sp_estatus_empleado_listar", commandType: CommandType.StoredProcedure);
        }
    }
}
