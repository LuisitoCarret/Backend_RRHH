using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Contratos.Application.Contracts;

public sealed class CreateRenewalRequest
{
    [JsonPropertyName("fechaRenovacion")] public DateTime FechaRenovacion { get; init; }
    [JsonPropertyName("nuevaFechaFin")] public DateTime NuevaFechaFin { get; init; }
    [JsonPropertyName("comentario")] public string? Comentario { get; init; }
}
