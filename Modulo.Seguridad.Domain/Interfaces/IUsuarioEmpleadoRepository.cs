namespace Modulo.Seguridad.Domain.Interfaces
{
    public interface IUsuarioEmpleadoRepository
    {
        Task CreateAsync(UsuarioEmpleado relacion);
        Task<long?> GetEmpleadoIdByUsuarioIdAsync(long usuarioId);

        Task<UsuarioEmpleado?> GetByEmpleadoIdAsync(int empleadoId);
        Task<bool> HasOtherEmployeesAsync(long usuarioId);
        Task DeleteAsync(long usuarioId, int empleadoId);
    }
}
