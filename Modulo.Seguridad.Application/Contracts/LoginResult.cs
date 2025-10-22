namespace Modulo.Seguridad.Application.Contracts
{
    public sealed class LoginResult
    {
        public long UsuarioId { get; set; }
        public string Email { get; set; } = "";
        public string[] Roles { get; set; } = Array.Empty<string>();
        public int? EmpleadoId { get; set; }
    }
}
