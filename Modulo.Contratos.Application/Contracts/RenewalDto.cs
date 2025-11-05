using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Contratos.Application.Contracts;

public sealed class RenewalDto
{
    [JsonPropertyName("renovacionId")] public int RenovacionId { get; init; }
    [JsonPropertyName("contratoId")] public int ContratoId { get; init; }
    [JsonPropertyName("fechaRenovacion")] public string FechaRenovacion { get; init; } = default!;
    [JsonPropertyName("nuevaFechaFin")] public string NuevaFechaFin { get; init; } = default!;
    [JsonPropertyName("comentario")] public string? Comentario { get; init; }
}
