using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Asistencias.Application.Contracts;

public sealed class AsistenciaListItemDto
{
    [JsonPropertyName("asistenciaId")] public int AsistenciaId { get; init; }
    [JsonPropertyName("empleadoId")] public int EmpleadoId { get; init; }
    [JsonPropertyName("nombreEmpleado")] public string NombreEmpleado { get; init; } = default!;
    [JsonPropertyName("turno")] public string Turno { get; init; } = default!;
    [JsonPropertyName("fecha")] public string Fecha { get; init; } = default!;
    [JsonPropertyName("horaInicioTurno")] public string HoraInicioTurno { get; init; } = default!;
    [JsonPropertyName("horaFinTurno")] public string HoraFinTurno { get; init; } = default!;
    [JsonPropertyName("toleranciaMinutos")] public int ToleranciaMinutos { get; init; }
    [JsonPropertyName("horaEntradaReal")] public string? HoraEntradaReal { get; init; }
    [JsonPropertyName("horaSalidaReal")] public string? HoraSalidaReal { get; init; }
    [JsonPropertyName("retardoMinutos")] public int? RetardoMinutos { get; init; }
    [JsonPropertyName("salidaAnticipadaMinutos")] public int? SalidaAnticipadaMinutos { get; init; }
    [JsonPropertyName("estado")] public string Estado { get; init; } = default!;
}
