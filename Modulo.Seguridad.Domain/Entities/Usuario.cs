namespace Modulo.Seguridad.Domain.Entities
{
    public class Usuario
    {
        public long UsuarioId { get; set; }
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Estatus { get; set; } = "activo";
    }
}
