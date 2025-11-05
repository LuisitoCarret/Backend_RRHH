namespace Modulo.Asistencias.Application.Contracts;

public sealed class AsistenciaInsertRequest
{
    [JsonPropertyName("tipoRegistro")] public string TipoRegistro { get; init; } = default!; // "Entrada" | "Salida"
}
