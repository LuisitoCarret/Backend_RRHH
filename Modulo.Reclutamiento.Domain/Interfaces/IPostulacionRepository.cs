using Modulo.Reclutamiento.Domain.Entities;

namespace Modulo.Reclutamiento.Domain.Interfaces
{
    public interface IPostulacionRepository
    {
        Task<PostulacionDetail?> CreateAsync(PostulacionCreate data, CancellationToken ct);
        Task<PostulacionListResponse> ListAsync(
                    string? vacanteNombre,
                    string? estatus,
                    int page,
                    int pageSize,
                    CancellationToken ct);
        Task<PostulacionDetails?> GetDetalleAsync(int postulacionId, CancellationToken ct);
        Task<PostulacionUpdateResult> UpdateAsync(
        int postulacionId,
        string estatus,
        string? observacion,
        CancellationToken ct);
    }
}
