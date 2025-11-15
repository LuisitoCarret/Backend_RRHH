namespace Modulo.Evaluaciones.Application.Services
{
    public class ListarEvaluacionesService
    {
        private readonly IEvaluacionesRepository _repository;

        public ListarEvaluacionesService(IEvaluacionesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ListarEvaluacionResponse>> HandleAsync(int? areaId, string? estatus)
        {
            var data = await _repository.ListarEvaluacionesAsync(areaId, estatus);

            return data.Select(x => new ListarEvaluacionResponse
            {
                EvaluacionId = x.EvaluacionId,
                EmpleadoId = x.EmpleadoId,
                NombreEmpleado = x.NombreEmpleado,
                PlantillaId = x.PlantillaId,
                NombrePlantilla = x.NombrePlantilla,
                AreaId = x.AreaId,
                NombreArea = x.NombreArea,
                PuntajeTotal = x.PuntajeTotal,
                NivelDesempeno = x.NivelDesempeno,
                Estatus = x.Estatus
            });
        }
    }
}
