namespace Modulo.Evaluaciones.Application.Services
{
    public class CrearEvaluacionService
    {
        private readonly IEvaluacionesRepository _repository;

        public CrearEvaluacionService(IEvaluacionesRepository repository)
        {
            _repository = repository;
        }

        public async Task<CrearEvaluacionResponse> HandleAsync(CrearEvaluacionRequest request)
        {
            var (evaluacionId, estatus, mensaje) =
                await _repository.CrearEvaluacionAsync(request.EmpleadoId, request.PlantillaId);

            return new CrearEvaluacionResponse
            {
                EvaluacionId = evaluacionId,
                Estatus = estatus,
                Mensaje = mensaje
            };
        }
    }
}
