namespace Modulo.Empleados.Application.Services
{
    public class CatalogoService
    {
        private readonly ICatalogoRepository _repo;

        public CatalogoService(ICatalogoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<dynamic>> ObtenerAreas() => await _repo.GetAreasAsync();
        public async Task<IEnumerable<dynamic>> ObtenerPuestos(long? areaId = null) => await _repo.GetPuestosAsync(areaId);
        public async Task<IEnumerable<dynamic>> ObtenerTurnos() => await _repo.GetTurnosAsync();
        public async Task<IEnumerable<dynamic>> ObtenerEstatus() => await _repo.GetEstatusAsync();
    }
}
