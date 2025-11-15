using System.Text.Json.Serialization;

namespace Modulo.Evaluaciones.Application.Contracts
{
    public class ActualizarEvaluacionRequest
    {
        public int EvaluacionId { get; set; }

        [JsonPropertyName("estatus")]
        public string Estatus { get; set; } = "";

        [JsonPropertyName("retroalimentacion")]
        public string? Retroalimentacion { get; set; }

        [JsonPropertyName("detalle")]
        public List<DetalleEvaluacionItem> Detalle { get; set; } = new();
    }

}
