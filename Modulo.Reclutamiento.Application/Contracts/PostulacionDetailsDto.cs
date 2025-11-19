namespace Modulo.Reclutamiento.Application.Contracts
{
    public class PostulacionDetailsDto
    {
        public int PostulacionId { get; set; }
        public int VacanteId { get; set; }
        public string NombreVacante { get; set; } = default!;
        public string VacanteEstatus { get; set; } = default!;
        public string NombreContacto { get; set; } = default!;
        public string? EmailContacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? CvUrl { get; set; }
        public string Estatus { get; set; } = default!;
        public string? Observacion { get; set; }
        public DateTime FechaPostulacion { get; set; }
    }
}
