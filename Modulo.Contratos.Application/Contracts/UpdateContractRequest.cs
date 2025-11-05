namespace Modulo.Contratos.Application.Contracts;

public sealed class UpdateContractRequest
{
    [JsonPropertyName("tipoContratoId")] public int? TipoContratoId { get; init; }
    [JsonPropertyName("fechaInicio")] public DateTime? FechaInicio { get; init; }
    [JsonPropertyName("fechaFin")] public DateTime? FechaFin { get; init; }
    [JsonPropertyName("salarioBase")] public decimal? SalarioBase { get; init; }
    [JsonPropertyName("observaciones")] public string? Observaciones { get; init; }
}
