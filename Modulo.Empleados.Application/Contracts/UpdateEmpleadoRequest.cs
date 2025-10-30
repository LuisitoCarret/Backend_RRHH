namespace Modulo.Empleados.Application.Contracts
{
    public class UpdateEmpleadoRequest
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public int AreaId { get; set; }
        public int PuestoId { get; set; }
        public int TurnoId { get; set; }
        public int EstatusId { get; set; }
    }
}
