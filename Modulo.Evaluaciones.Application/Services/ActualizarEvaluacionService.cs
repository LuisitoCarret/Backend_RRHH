namespace Modulo.Evaluaciones.Application.Services
{
    public class ActualizarEvaluacionService
    {
        private readonly IEvaluacionesRepository _repository;

        public ActualizarEvaluacionService(IEvaluacionesRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActualizarEvaluacionResponse> HandleAsync(ActualizarEvaluacionRequest request)
        {
            string detalleJson = JsonSerializer.Serialize(request.Detalle);

            var (evalId, puntaje, nivel, estatus, mensaje) =
                await _repository.ActualizarEvaluacionAsync(
                    request.EvaluacionId,
                    detalleJson,
                    request.Retroalimentacion ?? "",
                    request.Estatus
                );

            return new ActualizarEvaluacionResponse
            {
                EvaluacionId = evalId,
                PuntajeTotal = puntaje,
                NivelDesempeno = nivel,
                Estatus = estatus,
                Mensaje = mensaje
            };
        }
    }
}
