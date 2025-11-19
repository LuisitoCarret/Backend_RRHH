// Modulo.Evaluaciones.Application/Services/PlantillasQueryService.cs
using Modulo.Evaluaciones.Application.Contracts;
using Modulo.Evaluaciones.Domain.Interfaces;

namespace Modulo.Evaluaciones.Application.Services;
public sealed class PlantillasQueryService
{
    private readonly IEvaluacionesRepository _repo;
    public PlantillasQueryService(IEvaluacionesRepository repo) => _repo = repo;

    public async Task<List<PlantillaListItemDto>> ListAsync(int? areaId, bool? vigente, CancellationToken ct)
        => (await _repo.ListPlantillasAsync(areaId, vigente, ct))
            .Select(p => new PlantillaListItemDto
            {
                plantilla_id = p.PlantillaId,
                nombre = p.Nombre,
                area_id = p.AreaId,
                nombre_area = p.NombreArea,
                periodo_inicio = p.PeriodoInicio,
                periodo_fin = p.PeriodoFin,
                vigente = p.Vigente
            }).ToList();

    public async Task<PlantillaDetalleDto?> GetAsync(int plantillaId, CancellationToken ct)
    {
        var d = await _repo.GetPlantillaDetalleAsync(plantillaId, ct);
        if (d is null) return null;

        return new PlantillaDetalleDto
        {
            plantilla_id = d.PlantillaId,
            nombre = d.Nombre,
            descripcion = d.Descripcion,
            area_id = d.AreaId,
            nombre_area = d.NombreArea,
            periodo_inicio = d.PeriodoInicio,
            periodo_fin = d.PeriodoFin,
            vigente = d.Vigente,
            indicadores = d.Indicadores.Select(i => new PlantillaIndicadorDto
            {
                indicador_id = i.IndicadorId,
                catalogo_id = i.CatalogoId,
                nombre = i.NombreIndicador,
                ponderacion = i.Ponderacion
            }).ToList()
        };
    }
}
