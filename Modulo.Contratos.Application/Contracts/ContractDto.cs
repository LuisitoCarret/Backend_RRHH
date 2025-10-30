using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Contratos.Application.Contracts;

public sealed class ContractDto
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("empleadoId")] public int EmpleadoId { get; init; }
    [JsonPropertyName("tipoContratoId")] public int TipoContratoId { get; init; }
    [JsonPropertyName("estatusContratoId")] public int EstatusContratoId { get; init; }
    [JsonPropertyName("fechaInicio")] public string FechaInicio { get; init; } = default!;
    [JsonPropertyName("fechaFin")] public string? FechaFin { get; init; }
    [JsonPropertyName("salarioBase")] public decimal SalarioBase { get; init; }
    [JsonPropertyName("observaciones")] public string? Observaciones { get; init; }
}

