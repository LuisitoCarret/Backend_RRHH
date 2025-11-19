using Modulo.Reclutamiento.Domain.Entities;

namespace Modulo.Reclutamiento.Domain.Interfaces;

public interface IVacanteRepository
{
    Task<VacanteDetail> CreateAsync(VacanteCreate data, CancellationToken ct);
    Task<VacanteListResponse> ListAsync(string? estatus, int? areaId, int? puestoId, int page, int pageSize, CancellationToken ct);
    Task<VacanteDetail?> GetDetalleAsync(int vacanteId, CancellationToken ct);
    Task<VacanteDetail?> UpdateAsync(VacanteUpdate data, CancellationToken ct);
}