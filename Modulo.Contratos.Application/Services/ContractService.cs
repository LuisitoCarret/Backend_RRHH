using System.Security.Claims;

namespace Modulo.Contratos.Application.Services
{
    public class ContractService
    {
        private readonly IContractRepository _contractRepository;

        public ContractService(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<IEnumerable<ContractListDto>> ListarContratosAsync(
            DateTime? fechaInicioDesde = null,
            DateTime? fechaFinHasta = null,
            int? tipoContratoId = null,
            int? estatusContratoId = null)
        {
            return await _contractRepository.ListarContratosAsync(fechaInicioDesde, fechaFinHasta, tipoContratoId, estatusContratoId);
        }

        public async Task<ContractMeDto> ObtenerContratoVigenteActualAsync(ClaimsPrincipal user)
        {
            var empleadoIdClaim = user.FindFirst("empleado_id")?.Value;

            if (string.IsNullOrEmpty(empleadoIdClaim))
                return null;

            if (!int.TryParse(empleadoIdClaim, out int empleadoId))
                return null;

            return await _contractRepository.ObtenerContratoVigenteAsync(empleadoId);
        }

        public async Task<IEnumerable<TypeContractDto>> ListarTiposContratoAsync()
        {
            return await _contractRepository.ListarTiposContratoAsync();
        }

    }
}
