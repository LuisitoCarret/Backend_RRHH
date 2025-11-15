namespace Modulo.Evaluaciones.Application.Services
{
    public class ObtenerEmpleadoDisponibleService
    {
        private readonly IEvaluacionesRepository _repo;

        public ObtenerEmpleadoDisponibleService(IEvaluacionesRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<EmpleadoDisponibleResult>> HandleAsync(int plantillaId)
        {
            return await _repo.ObtenerEmpleadosDisponiblesAsync(plantillaId);
        }
    }
}
