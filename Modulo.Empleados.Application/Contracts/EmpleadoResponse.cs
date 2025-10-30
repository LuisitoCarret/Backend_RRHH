namespace Modulo.Empleados.Application.Contracts
{
    public class EmpleadoResponse
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public DateOnly FechaIngreso { get; set; }
        public int AreaId { get; set; }
        public int PuestoId { get; set; }
        public int TurnoId { get; set; }
        public int EstatusId { get; set; }
        public string Area { get; set; }
        public string Puesto { get; set; }
        public string Turno { get; set; }
        public string Estatus { get; set; }
    }
}
