namespace Modulo.Asistencias.Application.Contracts
{
    public class AsistenciaUpdateRequest
    {
        [JsonPropertyName("horaEntrada")] public string? HoraEntrada { get; init; }
        [JsonPropertyName("horaSalida")] public string? HoraSalida { get; init; }
        [JsonPropertyName("observaciones")] public string? Observaciones { get; init; }
    }
}
