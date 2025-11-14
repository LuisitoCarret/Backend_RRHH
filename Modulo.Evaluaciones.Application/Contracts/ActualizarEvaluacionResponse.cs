namespace Modulo.Evaluaciones.Application.Contracts
{
    public class ActualizarEvaluacionResponse
    {
        public int EvaluacionId { get; set; }
        public decimal? PuntajeTotal { get; set; }
        public string? NivelDesempeno { get; set; }
        public string Estatus { get; set; } = "";
        public string Mensaje { get; set; } = "";
    }
}
