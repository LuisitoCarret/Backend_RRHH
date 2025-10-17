namespace Modulo.Seguridad.Application.Services
{
    public interface IAuthUseCase
    {
        Task<(bool ok, LoginResult? data, string error)> LoginAsync(string email, string password);
    }

    public sealed class AuthUseCase : IAuthUseCase
    {
        private readonly IUsuarioRepository _repo;
        public AuthUseCase(IUsuarioRepository repo) => _repo = repo;

        public async Task<(bool ok, LoginResult? data, string error)> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, null, "Credenciales inválidas.");

            var user = await _repo.GetByEmailActivoAsync(email);
            if (user is null)
                return (false, null, "Usuario o contraseña incorrectos.");

            bool ok = BCryptNet.Verify(password, user.PasswordHash);

            if (!ok)
                return (false, null, "Usuario o contraseña incorrectos.");

            var roles = await _repo.GetRolesByUsuarioIdAsync(user.UsuarioId);

            return (true, new LoginResult
            {
                UsuarioId = user.UsuarioId,
                Email = user.Email,
                Roles = roles
            }, "");
        }
    }
}
