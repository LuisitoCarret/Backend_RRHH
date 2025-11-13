namespace Modulo.Contratos.Application.Contracts;

public sealed class RenewalDto
{
    [JsonPropertyName("renovacionId")] public int RenovacionId { get; init; }
    [JsonPropertyName("contratoId")] public int ContratoId { get; init; }
    [JsonPropertyName("empleadoId")] public int EmpleadoId { get; init; }
    [JsonPropertyName("fechaRenovacion")] public string FechaRenovacion { get; init; } = default!;
    [JsonPropertyName("fechaFinAnterior")] public string FechaFinAnterior { get; init; } = default!;
    [JsonPropertyName("nuevaFechaFin")] public string NuevaFechaFin { get; init; } = default!;
    [JsonPropertyName("comentario")] public string? Comentario { get; init; }
    [JsonPropertyName("mensaje")] public string? Mensaje { get; init; }
}
