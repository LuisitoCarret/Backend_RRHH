namespace Modulo.Contratos.Infrastructure.Repositories
{
    public class ContractDetailsRepository : IContractDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ContractDetailsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ContractDetailDto> ObtenerContratoDetalleAsync(int contratoId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var multi = await connection.QueryMultipleAsync(
                    "contratos.sp_contrato_detalle",
                    new { contrato_id = contratoId },
                    commandType: CommandType.StoredProcedure))
                {
                    var contrato = await multi.ReadFirstOrDefaultAsync<ContractDetailDto>();

                    if (contrato == null)
                        return null;

                    var renovaciones = (await multi.ReadAsync<RenewalDto>()).ToList();

                    contrato.Renovaciones = renovaciones;
                    return contrato;
                }
            }
        }
    }
}
