namespace Modulo.Contratos.Domain.Entities;

public sealed class RenovacionContrato
{
    public int RenovacionId { get; init; }
    public int ContratoId { get; init; }
    public string FechaRenovacion { get; init; } = default!;
    public string NuevaFechaFin { get; init; } = default!;
    public string? Comentario { get; init; }
}

