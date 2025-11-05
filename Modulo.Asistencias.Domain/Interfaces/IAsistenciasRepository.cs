using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modulo.Asistencias.Domain.Entities;

namespace Modulo.Asistencias.Domain.Interfaces;

public interface IAsistenciasRepository
{
    Task<IReadOnlyList<AsistenciaListado>> ListarAsync(DateTime? desde, DateTime? hasta, int? turnoId, CancellationToken ct);
    Task<AsistenciaRegistro> InsertarAsync(int empleadoId, string tipoRegistro, CancellationToken ct);
}
