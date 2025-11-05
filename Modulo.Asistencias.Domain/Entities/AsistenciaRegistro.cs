using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Asistencias.Domain.Entities;

public sealed class AsistenciaRegistro
{
    public int AsistenciaId { get; init; }
    public int EmpleadoId { get; init; }
    public string NombreEmpleado { get; init; } = default!;
    public string Turno { get; init; } = default!;
    public string Fecha { get; init; } = default!;
    public string HoraInicioTurno { get; init; } = default!;
    public string HoraFinTurno { get; init; } = default!;
    public int ToleranciaMinutos { get; init; }
    public string? HoraEntradaReal { get; init; }
    public string? HoraSalidaReal { get; init; }
    public int? RetardoMinutos { get; init; }
    public int? SalidaAnticipadaMinutos { get; init; }
    public string Estado { get; init; } = default!;
    public string Mensaje { get; init; } = default!;
}
