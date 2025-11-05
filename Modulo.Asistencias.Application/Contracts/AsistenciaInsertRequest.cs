using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Asistencias.Application.Contracts;

public sealed class AsistenciaInsertRequest
{
    [JsonPropertyName("empleadoId")] public int EmpleadoId { get; init; }
    [JsonPropertyName("tipoRegistro")] public string TipoRegistro { get; init; } = default!; // "Entrada" | "Salida"
}
