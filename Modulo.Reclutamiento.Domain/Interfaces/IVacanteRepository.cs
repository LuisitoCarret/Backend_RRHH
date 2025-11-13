using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Modulo.Reclutamiento.Domain.Interfaces;

public interface IVacanteRepository
{
    Task<VacanteDetail> CreateAsync(VacanteCreate data, CancellationToken ct);
    Task<VacanteListResponse> ListAsync(string? estatus, int? areaId, int? puestoId, int page, int pageSize, CancellationToken ct);
}