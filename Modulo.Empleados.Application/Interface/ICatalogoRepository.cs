namespace Modulo.Empleados.Application.Interface
{
    public interface ICatalogoRepository
    {
        Task<IEnumerable<dynamic>> GetAreasAsync();
        Task<IEnumerable<dynamic>> GetPuestosAsync(long? areaId = null);
        Task<IEnumerable<dynamic>> GetTurnosAsync();
        Task<IEnumerable<dynamic>> GetEstatusAsync();
    }
}
