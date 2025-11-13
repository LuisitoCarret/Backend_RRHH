namespace Modulo.Contratos.Infrastructure.Repositories
{
    public class EmployeeWithoutContractRepository : IEmployeeWithoutContract
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public EmployeeWithoutContractRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<EmployeeWithoutContractDto>> ObtenerEmpleadosSinContratoAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<EmployeeWithoutContractDto>(
                    "empleados.sp_empleados_con_contrato_vigente",
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
    }
}
