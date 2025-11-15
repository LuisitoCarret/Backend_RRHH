using Modulo.Evaluaciones.Domain.Entities;

namespace Modulo.Evaluaciones.Domain.Interfaces
{
    public interface IEvaluacionesRepository
    {

        Task<IReadOnlyList<CatalogoIndicador>> ListCatalogoAsync(CancellationToken ct);
        Task<IReadOnlyList<PlantillaListItem>> ListPlantillasAsync(int? areaId, bool? vigente, CancellationToken ct);
        Task<PlantillaDetalle?> GetPlantillaDetalleAsync(int plantillaId, CancellationToken ct);
        Task<(int plantillaId, string mensaje)> CrearPlantillaAsync(
            string nombre, string? descripcion, int areaId, DateTime periodoInicio, DateTime periodoFin,
            string indicadoresJson, CancellationToken ct);
        Task<string> ActualizarVigenciaAsync(int plantillaId, bool vigente, CancellationToken ct);

        Task<(int evaluacionId, string estatus, string mensaje)> CrearEvaluacionAsync(int empleadoId, int plantillaId);
        Task<(int evaluacionId, decimal? puntajeTotal, string? nivelDesempeno, string estatus, string mensaje)>
        ActualizarEvaluacionAsync(int evaluacionId, string detalleJson, string retroalimentacion, string estatus);
        Task<IEnumerable<ListarEvaluacionResult>> ListarEvaluacionesAsync(int? areaId, string? estatus);
        Task<DetalleEvaluacionResult?> ObtenerDetalleEvaluacionAsync(int evaluacionId);
        Task<IEnumerable<ListarEvaluacionResult>> ListarPorEmpleadoAsync(int empleadoId);
    }
}
