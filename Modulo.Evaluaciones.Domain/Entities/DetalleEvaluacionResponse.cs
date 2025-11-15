namespace Modulo.Evaluaciones.Domain.Entities
{
    public class DetalleEvaluacionResponse
    {
        public int EvaluacionId { get; set; }
        public string Empleado { get; set; } = "";
        public int AreaId { get; set; }
        public string NombreArea { get; set; } = "";
        public string Plantilla { get; set; } = "";
        public decimal? PuntajeTotal { get; set; }
        public string? NivelDesempeno { get; set; }
        public string? Retroalimentacion { get; set; }
        public string Estatus { get; set; } = "";
        public List<DetalleIndicadorItem> Detalle { get; set; } = new();
    }
}
