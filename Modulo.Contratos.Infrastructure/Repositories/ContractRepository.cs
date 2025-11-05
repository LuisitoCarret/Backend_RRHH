namespace Modulo.Contratos.Infrastructure.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ContractRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<ContractListDto>> ListarContratosAsync(DateTime? fechaInicioDesde = null,
            DateTime? fechaFinHasta = null, int? tipoContratoId = null, int? estatusContratoId = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@fecha_inicio_desde", fechaInicioDesde, DbType.Date);
                parameters.Add("@fecha_fin_hasta", fechaFinHasta, DbType.Date);
                parameters.Add("@tipo_contrato_id", tipoContratoId, DbType.Int32);
                parameters.Add("@estatus_contrato_id", estatusContratoId, DbType.Int32);

                var result = await connection.QueryAsync<ContractListDto>(
                    "contratos.sp_contratos_listar",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
        public async Task<ContractMeDto> ObtenerContratoVigenteAsync(int empleadoId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryFirstOrDefaultAsync<ContractMeDto>(
                    "contratos.sp_contrato_me",
                    new { empleado_id = empleadoId },
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }
                public async Task<IEnumerable<TypeContractDto>> ListarTiposContratoAsync()
               {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        var result = await connection.QueryAsync<TypeContractDto>(
                            "contratos.sp_tipos_contrato_listar",
                            commandType: CommandType.StoredProcedure
                        );

                        return result;
                    }
        }
    }
}
