namespace Modulo.Asistencias.Application.Contracts
{
    public class AsistenciaReporteMensualDto
    {
        [JsonPropertyName("empleadoId")] public int EmpleadoId { get; init; }
        [JsonPropertyName("nombreEmpleado")] public string NombreEmpleado { get; init; } = default!;
        [JsonPropertyName("diasAsistidos")] public int DiasAsistidos { get; init; }
        [JsonPropertyName("totalRetardos")] public int TotalRetardos { get; init; }
        [JsonPropertyName("pendientesDeSalida")] public int PendientesDeSalida { get; init; } 
        [JsonPropertyName("totalAusencias")] public int TotalAusencias { get; init; }
    }
}
