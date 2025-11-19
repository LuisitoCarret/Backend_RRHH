namespace Modulo.Evaluaciones.Application.Services
{
    public class ObtenerMisEvaluacionesService
    {
        private readonly IEvaluacionesRepository _repo;

        public ObtenerMisEvaluacionesService(IEvaluacionesRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ListarEvaluacionResponse>> HandleAsync(int empleadoId)
        {
            var data = await _repo.ListarPorEmpleadoAsync(empleadoId);

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
