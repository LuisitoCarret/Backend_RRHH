namespace Modulo.Evaluaciones.Domain.Entities
{
    public class ListarEvaluacionResult
    {
        public int EvaluacionId { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; } = "";
        public int PlantillaId { get; set; }
        public string NombrePlantilla { get; set; } = "";
        public int AreaId { get; set; }
        public string NombreArea { get; set; } = "";
        public decimal? PuntajeTotal { get; set; }
        public string? NivelDesempeno { get; set; }
        public string Estatus { get; set; } = "";
    }

    public class DetalleEvaluacionResult
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
        public IEnumerable<DetalleIndicadorItem> Detalle { get; set; } = new List<DetalleIndicadorItem>();
    }
}
