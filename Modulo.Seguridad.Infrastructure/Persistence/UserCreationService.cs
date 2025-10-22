namespace Modulo.Seguridad.Infrastructure.Persistence
{
    public class UserCreationService : IUserCreationService
    {
        private readonly SeguridadDbContext _context;

        public UserCreationService(SeguridadDbContext context)
        {
            _context = context;
        }
        public async Task<long> CreateUserWithRoleAsync(string email, string password, string roleSlug)
        {
            // Evitar duplicados
            var existing = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (existing != null)
                return (int)existing.UsuarioId;

            // Crear usuario
            var hashed = BCrypt.Net.BCrypt.HashPassword(password);
            var usuario = new Usuario
            {
                Email = email,
                PasswordHash = hashed,
                Nombre = email.Split('@')[0],
                Estatus = "activo"
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Asignar rol
            var rol = await _context.Roles.FirstOrDefaultAsync(r => r.Slug == roleSlug);
            if (rol != null)
            {
                _context.UsuariosRoles.Add(new UsuarioRol
                {
                    UsuarioId = usuario.UsuarioId,
                    RolId = rol.RolId
                });
                await _context.SaveChangesAsync();
            }

            return usuario.UsuarioId;
        }
    }
}
