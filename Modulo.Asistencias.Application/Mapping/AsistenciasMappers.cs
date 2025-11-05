namespace Modulo.Asistencias.Application.Mapping;

public static class AsistenciasMappers
{
    public static AsistenciaListItemDto ToDto(this AsistenciaListado x) => new()
    {
        AsistenciaId = x.AsistenciaId,
        EmpleadoId = x.EmpleadoId,
        NombreEmpleado = x.NombreEmpleado,
        Turno = x.Turno,
        Fecha = x.Fecha,
        HoraInicioTurno = x.HoraInicioTurno,
        HoraFinTurno = x.HoraFinTurno,
        ToleranciaMinutos = x.ToleranciaMinutos,
        HoraEntradaReal = x.HoraEntradaReal,
        HoraSalidaReal = x.HoraSalidaReal,
        RetardoMinutos = x.RetardoMinutos,
        SalidaAnticipadaMinutos = x.SalidaAnticipadaMinutos,
        Estado = x.Estado
    };

    public static AsistenciaInsertResponseDto ToDto(this AsistenciaRegistro x) => new()
    {
        AsistenciaId = x.AsistenciaId,
        EmpleadoId = x.EmpleadoId,
        NombreEmpleado = x.NombreEmpleado,
        Turno = x.Turno,
        Fecha = x.Fecha,
        HoraInicioTurno = x.HoraInicioTurno,
        HoraFinTurno = x.HoraFinTurno,
        ToleranciaMinutos = x.ToleranciaMinutos,
        HoraEntradaReal = x.HoraEntradaReal,
        HoraSalidaReal = x.HoraSalidaReal,
        RetardoMinutos = x.RetardoMinutos,
        SalidaAnticipadaMinutos = x.SalidaAnticipadaMinutos,
        Estado = x.Estado,
        Mensaje = x.Mensaje
    };

    public static AsistenciaInsertResponseDto ToDto(this AsistenciaActualizada x) => new()
    {
        AsistenciaId = x.AsistenciaId,
        EmpleadoId = x.EmpleadoId,
        NombreEmpleado = x.NombreEmpleado,
        Turno = x.Turno,
        Fecha = x.Fecha,
        HoraInicioTurno = x.HoraInicioTurno,
        HoraFinTurno = x.HoraFinTurno,
        ToleranciaMinutos = x.ToleranciaMinutos,
        HoraEntradaReal = x.HoraEntradaReal,
        HoraSalidaReal = x.HoraSalidaReal,
        RetardoMinutos = x.RetardoMinutos,
        Estado = x.Estado,
        Mensaje = x.Mensaje
    };

    public static AsistenciaReporteMensualDto ToDto(this AsistenciaReporteMensual x) => new()
    {
        EmpleadoId = x.EmpleadoId,
        NombreEmpleado = x.NombreEmpleado,
        DiasAsistidos = x.DiasAsistidos,
        TotalRetardos = x.TotalRetardos,
        PendientesDeSalida = x.PendientesDeSalida,
        TotalAusencias = x.TotalAusencias
    };

}
