using Modulo.Asistencias.Domain.Entities;

namespace Modulo.Asistencias.Domain.Interfaces;

public interface IAsistenciasRepository
{
    Task<IReadOnlyList<AsistenciaListado>> ListarAsync(DateTime? desde, DateTime? hasta, int? turnoId, CancellationToken ct);
    Task<AsistenciaRegistro> InsertarAsync(int empleadoId, string tipoRegistro, CancellationToken ct);

    Task<AsistenciaActualizada> ActualizarAsync(int asistenciaId, TimeSpan? horaEntrada, TimeSpan? horaSalida, string? observaciones, CancellationToken ct);
    Task<IReadOnlyList<AsistenciaReporteMensual>> ReporteMensualAsync(DateTime fechaInicio, DateTime fechaFin, CancellationToken ct);
}
