namespace Modulo.Evaluaciones.Application.Services
{
    public class ObtenerDetalleEvaluacionService
    {
       private readonly IEvaluacionesRepository _repository;

        public ObtenerDetalleEvaluacionService(IEvaluacionesRepository repository)
        {
            _repository = repository;
        }

        public async Task<DetalleEvaluacionResponse?> HandleAsync(int evaluacionId)
        {
            var data = await _repository.ObtenerDetalleEvaluacionAsync(evaluacionId);

            if (data == null)
                return null;

            return new DetalleEvaluacionResponse
            {
                EvaluacionId = data.EvaluacionId,
                Empleado = data.Empleado,
                AreaId = data.AreaId,
                NombreArea = data.NombreArea,
                Plantilla = data.Plantilla,
                PuntajeTotal = data.PuntajeTotal,
                NivelDesempeno = data.NivelDesempeno,
                Retroalimentacion = data.Retroalimentacion,
                Estatus = data.Estatus,
                Detalle = data.Detalle.ToList()
            };
        }
    }
}
