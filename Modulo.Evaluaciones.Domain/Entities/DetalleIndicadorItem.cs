using System.Text.Json.Serialization;

namespace Modulo.Evaluaciones.Domain.Entities
{
    public class DetalleIndicadorItem
    {
        public string Indicador { get; set; } = "";
        public decimal Ponderacion { get; set; }
        public decimal? Calificacion { get; set; }
    }
}
