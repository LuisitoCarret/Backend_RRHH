namespace Modulo.Reclutamiento.Application.Services;

public sealed class VacantesService
{
    private readonly IVacanteRepository _repo;
    public VacantesService(IVacanteRepository repo) => _repo = repo;

    public async Task<VacanteDetailDto> CreateAsync(CreateVacanteRequest req, CancellationToken ct)
    {
        var created = await _repo.CreateAsync(new VacanteCreate
        {
            Titulo = req.Titulo,
            Descripcion = req.Descripcion,
            AreaId = req.AreaId,
            PuestoId = req.PuestoId,
            FechaPublicacion = req.FechaPublicacion
        }, ct);

        return new VacanteDetailDto
        {
            VacanteId = created.VacanteId,
            Titulo = created.Titulo,
            Descripcion = created.Descripcion,
            AreaId = created.AreaId,
            NombreArea = created.NombreArea,
            PuestoId = created.PuestoId,
            NombrePuesto = created.NombrePuesto,
            Estatus = created.Estatus,
            FechaPublicacion = created.FechaPublicacion,
            FechaCierre = created.FechaCierre
        };
    }

    public async Task<ListVacantesResponseDto> ListAsync(ListVacantesRequest req, CancellationToken ct)
    {
        var result = await _repo.ListAsync(req.Estatus, req.AreaId, req.PuestoId, req.Page, req.PageSize, ct);

        return new ListVacantesResponseDto
        {
            Page = result.Page,
            PageSize = result.PageSize,
            Total = result.Total,
            Items = result.Items.Select(x => new VacanteItemDto
            {
                VacanteId = x.VacanteId,
                Titulo = x.Titulo,
                AreaId = x.AreaId,
                NombreArea = x.NombreArea,
                PuestoId = x.PuestoId,
                NombrePuesto = x.NombrePuesto,
                Estatus = x.Estatus,
                FechaPublicacion = x.FechaPublicacion
            }).ToList()
        };
    }

    public async Task<VacanteDetailDto> GetDetalleAsync(int vacanteId, CancellationToken ct)
    {
        if (vacanteId <= 0)
            throw new BusinessRuleException("La vacante solicitada no existe.");

        var detail = await _repo.GetDetalleAsync(vacanteId, ct);

        if (detail is null)
            throw new BusinessRuleException("La vacante no existe.");

        return new VacanteDetailDto
        {
            VacanteId = detail.VacanteId,
            Titulo = detail.Titulo,
            Descripcion = detail.Descripcion,
            AreaId = detail.AreaId,
            NombreArea = detail.NombreArea,
            PuestoId = detail.PuestoId,
            NombrePuesto = detail.NombrePuesto,
            Estatus = detail.Estatus,
            FechaPublicacion = detail.FechaPublicacion,
            FechaCierre = detail.FechaCierre
        };
    }

    public async Task<VacanteDetailDto> UpdateAsync(int vacanteId, UpdateVacanteRequest req, CancellationToken ct)
    {
        if (vacanteId <= 0)
            throw new BusinessRuleException("No se pudo actualizar la vacante.");

        var updated = await _repo.UpdateAsync(new VacanteUpdate
        {
            VacanteId = vacanteId,
            Titulo = req.Titulo,
            Descripcion = req.Descripcion,
            AreaId = req.AreaId,
            PuestoId = req.PuestoId

        }, ct);

        if (updated is null)
            throw new BusinessRuleException("No se pudo actualizar la vacante.");

        return new VacanteDetailDto
        {
            VacanteId = updated.VacanteId,
            Titulo = updated.Titulo,
            Descripcion = updated.Descripcion,
            AreaId = updated.AreaId,
            NombreArea = updated.NombreArea,
            PuestoId = updated.PuestoId,
            NombrePuesto = updated.NombrePuesto,
            Estatus = updated.Estatus,
            FechaPublicacion = updated.FechaPublicacion,
            FechaCierre = updated.FechaCierre
        };
    }

}
