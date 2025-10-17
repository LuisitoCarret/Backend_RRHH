namespace Modulo.Seguridad.Application.Contracts
{
    public sealed class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}

