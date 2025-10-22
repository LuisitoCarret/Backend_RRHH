namespace Modulo.Seguridad.Domain.Entities
{
    public class UsuarioRol
    {
        public long UsuarioId { get; set; }
        public long RolId { get; set; }

        public Rol Rol { get; set; }
        public Usuario Usuario { get; set; }
    }
}
