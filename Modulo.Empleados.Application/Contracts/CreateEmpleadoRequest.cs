    namespace Modulo.Empleados.Application.Dto
{
    public class CreateEmpleadoRequest
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaIngreso { get; set; }
        public int AreaId { get; set; }
        public int PuestoId { get; set; }
        public int TurnoId { get; set; }
        public int EstatusId { get; set; }

        public string Password { get; set; }
        public string Rol { get; set; }

        public DomicilioRequest Domicilio { get; set; } = new();

        public ContactoEmergenciaRequest Contacto { get; set; } = new();

        public sealed class DomicilioRequest
        {
            public string Calle { get; set; }
            public string? Numero { get; set; }
            public string Colonia { get; set; } 
            public string Ciudad { get; set; } 
            public string Estado { get; set; } 
            public string CodigoPostal { get; set; } 
        }

        public sealed class ContactoEmergenciaRequest
        {
            public string Nombre { get; set; } 
            public string Parentesco { get; set; } 
            public string Telefono { get; set; } 
        }
    }
}
