namespace Modulo.Contratos.Domain.Entities;

public sealed class Contrato
{
    public int Id { get; init; }
    public int EmpleadoId { get; init; }
    public string? NombreEmpleado { get; init; } 
    public int TipoContratoId { get; init; }
    public string? TipoContratoNombre { get; init; }
    public int EstatusContratoId { get; init; }
    public string? EstatusContratoNombre { get; init; }
    public string FechaInicio { get; init; } = default!; // yyyy-MM-dd
    public string? FechaFin { get; init; }
    public decimal SalarioBase { get; init; }
    public string? Observaciones { get; init; }

}

