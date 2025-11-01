namespace Modulo.Contratos.Application.Interfaces
{
    public interface IContractRepository
    {
        Task<IEnumerable<ContractListDto>> ListarContratosAsync(
            DateTime? fechaInicioDesde = null,
            DateTime? fechaFinHasta = null,
            int? tipoContratoId = null,
            int? estatusContratoId = null);

        Task<ContractMeDto> ObtenerContratoVigenteAsync(int empleadoId);

        Task<IEnumerable<TypeContractDto>> ListarTiposContratoAsync();
    }
}
