namespace Modulo.Asistencias.Domain.Entities
{
    public class AsistenciaReporteMensual
    {
        public int EmpleadoId { get; init; }
        public string NombreEmpleado { get; init; } = default!;
        public int DiasAsistidos { get; init; }
        public int TotalRetardos { get; init; }
        public int PendientesDeSalida { get; init; }
        public int TotalAusencias { get; init; }
    }
}
