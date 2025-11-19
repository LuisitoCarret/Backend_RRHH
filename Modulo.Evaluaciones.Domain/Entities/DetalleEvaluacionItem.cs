using System.Text.Json.Serialization;

namespace Modulo.Evaluaciones.Domain.Entities
{
    public class DetalleEvaluacionItem
    {
        [JsonPropertyName("indicador_id")]
        public int IndicadorId { get; set; }

        [JsonPropertyName("calificacion")]
        public decimal? Calificacion { get; set; }

        [JsonPropertyName("comentarios")]
        public string? Comentarios { get; set; }
    }
}
