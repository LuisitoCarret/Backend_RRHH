namespace Modulo.Reclutamiento.Domain.Entities
{
    public class PostulacionListItem
    {
        public int PostulacionId { get; set; }
        public int VacanteId { get; set; }
        public string NombreVacante { get; set; } = default!;
        public string NombreContacto { get; set; } = default!;
        public string Estatus { get; set; } = default!;
        public DateTime FechaPostulacion { get; set; }
    }
}
