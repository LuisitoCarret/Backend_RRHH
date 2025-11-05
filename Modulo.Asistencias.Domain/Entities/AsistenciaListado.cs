using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Asistencias.Domain.Entities;

public sealed class AsistenciaListado
{
    public int AsistenciaId { get; init; }
    public int EmpleadoId { get; init; }
    public string NombreEmpleado { get; init; } = default!;
    public string Turno { get; init; } = default!;
    public string Fecha { get; init; } = default!;              // yyyy-MM-dd
    public string HoraInicioTurno { get; init; } = default!;    // HH:mm:ss
    public string HoraFinTurno { get; init; } = default!;       // HH:mm:ss
    public int ToleranciaMinutos { get; init; }
    public string? HoraEntradaReal { get; init; }               // HH:mm:ss | null
    public string? HoraSalidaReal { get; init; }                // HH:mm:ss | null
    public int? RetardoMinutos { get; init; }                   // null si no hay entrada
    public int? SalidaAnticipadaMinutos { get; init; }          // null si no hay salida
    public string Estado { get; init; } = default!;
}

