using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Contratos.Application.Contracts;

public sealed class UpdateContractRequest
{
    [JsonPropertyName("tipoContratoId")] public int TipoContratoId { get; init; }
    [JsonPropertyName("estatusContratoId")] public int EstatusContratoId { get; init; }
    [JsonPropertyName("fechaInicio")] public DateTime FechaInicio { get; init; }
    [JsonPropertyName("fechaFin")] public DateTime? FechaFin { get; init; }
    [JsonPropertyName("salarioBase")] public decimal SalarioBase { get; init; }
    [JsonPropertyName("observaciones")] public string? Observaciones { get; init; }
}
