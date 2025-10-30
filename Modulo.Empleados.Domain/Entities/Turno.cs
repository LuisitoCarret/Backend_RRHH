namespace Modulo.Empleados.Domain.Entities
{
    public class Turno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin {  get; set; }
        public int ToleranciaMinutos { get; set; }
    }
}
