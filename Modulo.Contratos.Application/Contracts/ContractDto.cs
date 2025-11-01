namespace Modulo.Contratos.Application.Contracts;

public sealed class ContractDto
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("empleadoId")] public int EmpleadoId { get; init; }
    [JsonPropertyName("nombreEmpleado")] public string? NombreEmpleado { get; init; }
    [JsonPropertyName("tipoContratoId")] public int TipoContratoId { get; init; }
    [JsonPropertyName("tipoContratoNombre")] public string? TipoContratoNombre { get; init; }
    [JsonPropertyName("estatusContratoId")] public int EstatusContratoId { get; init; }
    [JsonPropertyName("estatusContratoNombre")] public string? EstatusContratoNombre { get; init; }
    [JsonPropertyName("fechaInicio")] public string FechaInicio { get; init; } = default!;
    [JsonPropertyName("fechaFin")] public string? FechaFin { get; init; }
    [JsonPropertyName("salarioBase")] public decimal SalarioBase { get; init; }
    [JsonPropertyName("observaciones")] public string? Observaciones { get; init; }
}

