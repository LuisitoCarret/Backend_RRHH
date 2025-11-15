using Modulo.Evaluaciones.Domain.Entities;

namespace Modulo.Evaluaciones.Domain.Interfaces
{
    public interface IEvaluacionesRepository
    {
        Task<(int evaluacionId, string estatus, string mensaje)> CrearEvaluacionAsync(int empleadoId, int plantillaId);
        Task<(int evaluacionId, decimal? puntajeTotal, string? nivelDesempeno, string estatus, string mensaje)>
        ActualizarEvaluacionAsync(int evaluacionId, string detalleJson, string retroalimentacion, string estatus);
        Task<IEnumerable<ListarEvaluacionResult>> ListarEvaluacionesAsync(int? areaId, string? estatus);
        Task<DetalleEvaluacionResult?> ObtenerDetalleEvaluacionAsync(int evaluacionId);
        Task<IEnumerable<ListarEvaluacionResult>> ListarPorEmpleadoAsync(int empleadoId);
    }
}
