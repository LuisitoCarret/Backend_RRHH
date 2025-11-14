namespace Modulo.Evaluaciones.Domain.Entities
{
    public class ListarEvaluacionResponse
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
}
