namespace Modulo.Seguridad.Infrastructure.Persistence
{
    public sealed class UsuarioRepository : IUsuarioRepository
    {
        private readonly SeguridadDbContext _context;

        public UsuarioRepository(SeguridadDbContext context)
        {
            _context = context;
        }
        public async Task<Usuario?> GetByEmailActivoAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.Estatus == "activo");
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<string[]> GetRolesByUsuarioIdAsync(long usuarioId)
        {
            return await _context.UsuariosRoles
                .Where(ur => ur.UsuarioId == usuarioId)
                .Select(ur => ur.Rol.Slug)
                .ToArrayAsync();
        }

        public async Task DeleteRolesByUsuarioIdAsync(long usuarioId)
        {
            var roles = await _context.UsuariosRoles
        .Where(ur => ur.UsuarioId == usuarioId)
        .ToListAsync();

            if (roles.Count > 0)
            {
                _context.UsuariosRoles.RemoveRange(roles);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(long usuarioId)
        {
            var usuario = await _context.Usuarios
              .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
