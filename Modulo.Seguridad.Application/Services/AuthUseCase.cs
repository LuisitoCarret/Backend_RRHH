namespace Modulo.Seguridad.Application.Services
{
    public interface IAuthUseCase
    {
        Task<(bool ok, LoginResult? data, LoginFailureReason? reason, string error)> LoginAsync(string email, string password);
    }

    public sealed class AuthUseCase : IAuthUseCase
    {
        private readonly IUsuarioRepository _repo;
        public AuthUseCase(IUsuarioRepository repo) => _repo = repo;

        public async Task<(bool ok, LoginResult? data, LoginFailureReason? reason, string error)> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, null, LoginFailureReason.InvalidInput, "Credenciales inválidas");

            var normalizedEmail = email.Trim();

            var user = await _repo.GetByEmailAsync(normalizedEmail);

            if (user is null)
                return (false, null, LoginFailureReason.InvalidEmail, "Correo no registrado");

            if (!string.Equals(user.Estatus, "activo", StringComparison.OrdinalIgnoreCase))
                return (false, null, LoginFailureReason.Inactive, "Usuario inactivo. Contacte al administrador");

            bool passOk = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!passOk)
                return (false, null, LoginFailureReason.InvalidPassword, "Contraseña incorrecta");

            var roles = await _repo.GetRolesByUsuarioIdAsync(user.UsuarioId);

            var lr = new LoginResult
            {
                UsuarioId = user.UsuarioId,
                Email = user.Email,
                Roles = roles
            };

            return (true, lr, LoginFailureReason.None, string.Empty);
        }
    }
}
