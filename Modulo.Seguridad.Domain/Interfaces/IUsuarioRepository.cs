using Modulo.Seguridad.Domain.Entities;

namespace Modulo.Seguridad.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByEmailActivoAsync(string email);
        Task<string[]> GetRolesByUsuarioIdAsync(long usuarioId);
    }
}
