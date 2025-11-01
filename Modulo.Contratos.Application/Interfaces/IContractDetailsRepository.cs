namespace Modulo.Contratos.Application.Interfaces
{
    public interface IContractDetailsRepository
    {
        Task<ContractDetailDto> ObtenerContratoDetalleAsync(int contratoId);
    }
}
