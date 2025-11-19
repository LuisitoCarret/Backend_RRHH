namespace Modulo.Reclutamiento.Application.Contracts
{
    public class CreatePostulacionRequest
    {
        public int VacanteId { get; set; }
        public string NombreContacto { get; set; } = default!;
        public string? EmailContacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public IFormFile? CvFile { get; set; }
        public string? Estatus { get; set; } 
        public string? Observacion { get; set; }
        public DateTime? FechaPostulacion { get; set; }
    }
}
