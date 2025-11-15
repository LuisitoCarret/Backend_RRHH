// Modulo.Evaluaciones.Application/Services/IndicadoresCatalogoService.cs
using Modulo.Evaluaciones.Application.Contracts;
using Modulo.Evaluaciones.Domain.Interfaces;

namespace Modulo.Evaluaciones.Application.Services;
public sealed class IndicadoresCatalogoService
{
    private readonly IEvaluacionesRepository _repo;
    public IndicadoresCatalogoService(IEvaluacionesRepository repo) => _repo = repo;

    public async Task<List<IndicadoresCatalogoDto>> GetAllAsync(CancellationToken ct)
        => (await _repo.ListCatalogoAsync(ct))
            .Select(x => new IndicadoresCatalogoDto
            {
                catalogo_id = x.CatalogoId,
                nombre = x.Nombre,
                descripcion = x.Descripcion
            }).ToList();
}
