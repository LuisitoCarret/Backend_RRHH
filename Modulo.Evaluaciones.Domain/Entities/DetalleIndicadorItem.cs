using System.Text.Json.Serialization;

namespace Modulo.Evaluaciones.Domain.Entities
{
    public class DetalleIndicadorItem
    {
        [JsonPropertyName("indicador_id")]
        public int IndicadorId { get; set; }

        [JsonPropertyName("indicador")]
        public string Indicador { get; set; } = "";

        [JsonPropertyName("ponderacion")]
        public decimal Ponderacion { get; set; }

        [JsonPropertyName("calificacion")]
        public decimal? Calificacion { get; set; }
    }
}
