// Modulo.Evaluaciones.Application/Services/PlantillasCommandService.cs
using System.Text.Json;
using Modulo.Evaluaciones.Application.Contracts;
using Modulo.Evaluaciones.Domain.Interfaces;

namespace Modulo.Evaluaciones.Application.Services;
public sealed class PlantillasCommandService
{
    private readonly IEvaluacionesRepository _repo;
    public PlantillasCommandService(IEvaluacionesRepository repo) => _repo = repo;

    public async Task<CreatePlantillaResponse> CreateAsync(CreatePlantillaRequest req, CancellationToken ct)
    {
        // Serializa el arreglo de indicadores al JSON esperado por el SP
        var json = JsonSerializer.Serialize(req.indicadores);
        var (id, mensaje) = await _repo.CrearPlantillaAsync(
            req.nombre, req.descripcion, req.area_id, req.periodo_inicio, req.periodo_fin, json, ct);

        return new CreatePlantillaResponse { plantilla_id = id, mensaje = mensaje };
    }

    public async Task<UpdateVigenciaResponse> UpdateVigenciaAsync(int plantillaId, bool vigente, CancellationToken ct)
    {
        var msg = await _repo.ActualizarVigenciaAsync(plantillaId, vigente, ct);
        return new UpdateVigenciaResponse { mensaje = msg };
    }
}
