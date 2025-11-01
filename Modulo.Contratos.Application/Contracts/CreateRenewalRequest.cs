namespace Modulo.Contratos.Application.Contracts;

public sealed class CreateRenewalRequest
{
    [JsonPropertyName("fechaRenovacion")] public DateTime FechaRenovacion { get; init; }
    [JsonPropertyName("nuevaFechaFin")] public DateTime NuevaFechaFin { get; init; }
    [JsonPropertyName("comentario")] public string? Comentario { get; init; }
}
