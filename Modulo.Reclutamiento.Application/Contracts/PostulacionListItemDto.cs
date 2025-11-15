namespace Modulo.Reclutamiento.Application.Contracts
{
    public class PostulacionListItemDto
    {
        public int PostulacionId { get; set; }
        public int VacanteId { get; set; }
        public string NombreVacante { get; set; } = default!;
        public string NombreContacto { get; set; } = default!;
        public string Estatus { get; set; } = default!;
        public DateTime FechaPostulacion { get; set; }
    }
}
