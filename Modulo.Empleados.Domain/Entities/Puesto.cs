namespace Modulo.Empleados.Domain.Entities
{
    public class Puesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int AreaId { get; set; }
        public Area Area { get; set; }
    }
}
