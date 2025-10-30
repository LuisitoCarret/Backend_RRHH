namespace Modulo.Empleados.Domain.Entities
{
    public class ContactoEmergencia
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string Parentesco { get; set; }
        public string Telefono { get; set; }
        public Empleado Empleado { get; set; }
    }
}
