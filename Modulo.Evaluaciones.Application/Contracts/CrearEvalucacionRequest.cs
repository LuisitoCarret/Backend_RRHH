using System.Text.Json.Serialization;

namespace Modulo.Evaluaciones.Application.Contracts
{
    public class CrearEvaluacionRequest
    {
        [JsonPropertyName("empleado_id")]
        public int EmpleadoId { get; set; }

        [JsonPropertyName("plantilla_id")]
        public int PlantillaId { get; set; }
    }
}