using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ==========================
// Modulo.Reclutamiento.Application/Services/VacantesService.cs
// ==========================
using Modulo.Reclutamiento.Domain;
using Modulo.Reclutamiento.Domain.Interfaces;
using Modulo.Reclutamiento.Application.Contracts;

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
}
