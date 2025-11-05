namespace Modulo.Contratos.Application.Services
{
    public class ContractDetailsService
    {
        private readonly IContractDetailsRepository _contractRepository;

        public ContractDetailsService(IContractDetailsRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<ContractDetailDto> ObtenerDetalleAsync(int contratoId)
        {
            return await _contractRepository.ObtenerContratoDetalleAsync(contratoId);
        }
    }
}
