namespace Modulo.Empleados.Domain.Entities
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaIngreso { get; set; }
        public int AreaId { get; set; }
        public int PuestoId { get; set; }
        public int TurnoId { get; set; }
        public int EstatusId { get; set; }
        public Area Area { get; set; }
        public Puesto Puesto { get; set; }
        public Turno Turno { get;set; }
        public EstatusEmpleado Estatus { get; set; }
        public ContactoEmergencia ContactoEmergencia {  get; set; }
        public DomicilioEmpleado Domicilio { get; set; }
    }
}
