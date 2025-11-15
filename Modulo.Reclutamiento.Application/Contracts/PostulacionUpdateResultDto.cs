namespace Modulo.Reclutamiento.Application.Contracts
{
    public class PostulacionUpdateResultDto
    {
        public int PostulacionId { get; set; }
        public int VacanteId { get; set; }
        public string Estatus { get; set; } = default!;
        public string? Observacion { get; set; }

        public string? VacanteEstatus { get; set; }
        public DateTime? VacanteFechaCierre { get; set; }
    }
}
