namespace Modulo.Evaluaciones.Domain.Entities
{
    public class Evaluacion
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public int PlantillaId { get; set; }
        public decimal? PuntajeTotal { get; set; }
        public string? NivelDesempeno { get; set; }
        public string? Retroalimentacion { get; set; }
        public string Estatus { get; set; } = "en_proceso";
    }
}
