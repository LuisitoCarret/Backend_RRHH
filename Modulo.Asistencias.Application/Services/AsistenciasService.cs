using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modulo.Asistencias.Application.Contracts;
using Modulo.Asistencias.Application.Mapping;
using Modulo.Asistencias.Domain.Interfaces;

namespace Modulo.Asistencias.Application.Services;

public sealed class AsistenciasService
{
    private readonly IAsistenciasRepository _repo;
    public AsistenciasService(IAsistenciasRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<AsistenciaListItemDto>> ListarAsync(DateTime? desde, DateTime? hasta, int? turnoId, CancellationToken ct)
        => (await _repo.ListarAsync(desde, hasta, turnoId, ct)).Select(r => r.ToDto()).ToList();

    public async Task<AsistenciaInsertResponseDto> InsertarAsync(AsistenciaInsertRequest req, CancellationToken ct)
        => (await _repo.InsertarAsync(req.EmpleadoId, req.TipoRegistro, ct)).ToDto();
}
