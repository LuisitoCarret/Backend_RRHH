namespace Modulo.Contratos.Application.Services;

public sealed class ContratoCommandService
{
    private readonly IContratoRepository _repo;
    public ContratoCommandService(IContratoRepository repo) => _repo = repo;

    public async Task<ContractDto> CreateAsync(CreateContractRequest req, CancellationToken ct)
        => (await _repo.InsertAsync(req.ToEntity(), ct)).ToDto();

    public async Task<ContractDto?> UpdateAsync(int contratoId, int empleadoId, UpdateContractRequest req, CancellationToken ct)
        => (await _repo.UpdateAsync(contratoId, req.ToEntity(contratoId, empleadoId), ct))?.ToDto();

    public async Task<bool> DeleteAsync(int contratoId, CancellationToken ct)
        => await _repo.DeleteAsync(contratoId, ct);

    public async Task<RenewalDto> RenewAsync(int contratoId, CreateRenewalRequest req, CancellationToken ct)
        => (await _repo.RenewAsync(contratoId, req.FechaRenovacion, req.NuevaFechaFin, req.Comentario, ct)).ToDto();
}
