namespace Modulo.Seguridad.Infrastructure.Persistence
{
    public sealed class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        public UsuarioRepository(string connectionString) => _connectionString = connectionString;

        public async Task<Usuario?> GetByEmailActivoAsync(string email)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT usuario_id AS UsuarioId, email, password_hash AS PasswordHash, nombre, estatus
                  FROM seguridad.usuarios
                  WHERE email = @email AND estatus = 'activo';",
                new { email });
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                @"SELECT usuario_id AS UsuarioId, email, password_hash AS PasswordHash, nombre, estatus
                  FROM seguridad.usuarios
                  WHERE LOWER(email) = LOWER(@email);",
                new { email = email?.Trim() });
        }


        public async Task<string[]> GetRolesByUsuarioIdAsync(long usuarioId)
        {
            using IDbConnection conn = new SqlConnection(_connectionString);
            var rows = await conn.QueryAsync<string>(
                @"SELECT r.slug
                  FROM seguridad.roles r
                  JOIN seguridad.usuarios_roles ur ON ur.rol_id = r.rol_id
                  WHERE ur.usuario_id = @usuarioId;",
                new { usuarioId });

            return rows?.ToArray() ?? Array.Empty<string>();
        }
    }
}
