namespace Modulo.Seguridad.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailActivoAsync(string email);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<string[]> GetRolesByUsuarioIdAsync(long usuarioId);
        Task DeleteAsync(long usuarioId);
        Task DeleteRolesByUsuarioIdAsync(long usuarioId);
    }
}
