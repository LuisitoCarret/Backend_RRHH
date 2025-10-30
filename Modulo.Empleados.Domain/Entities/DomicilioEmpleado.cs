namespace Modulo.Empleados.Domain.Entities
{
    public class DomicilioEmpleado
    {
        public int Id { get; set; }
        public string Calle {  get; set; }
        public string Numero { get; set; }
        public string Colonia { get; set; }
        public string Ciudad {  get; set; }
        public string Estado { get; set; }
        public string CodigoPostal { get; set; }
         public int EmpleadoId { get; set; }
        public Empleado Empleado {  get; set; }
    }
}
