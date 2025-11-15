namespace Modulo.Reclutamiento.Domain.Entities
{
    public class VacanteUpdate
    {
        public int VacanteId { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public int? AreaId { get; set; }
        public int? PuestoId { get; set; }
    }
}
