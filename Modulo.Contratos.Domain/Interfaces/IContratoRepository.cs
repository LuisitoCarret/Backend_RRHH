using Modulo.Contratos.Domain.Entities;

namespace Modulo.Contratos.Domain.Interfaces;

public interface IContratoRepository
{
    Task<Contrato> InsertAsync(Contrato data, CancellationToken ct);
    Task<Contrato?> UpdateAsync(int contratoId, Contrato data, CancellationToken ct);
    Task<bool> DeleteAsync(int contratoId, CancellationToken ct);
    Task<RenovacionContrato> RenewAsync(int contratoId, DateTime fechaRenovacion, DateTime nuevaFechaFin, string? comentario, CancellationToken ct);
}

