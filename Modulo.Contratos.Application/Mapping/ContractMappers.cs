namespace Modulo.Contratos.Application.Mapping;

public static class ContractMappers
{
    public static Contrato ToEntity(this CreateContractRequest r) => new()
    {
        EmpleadoId = r.EmpleadoId,
        TipoContratoId = r.TipoContratoId, // viene del front
        FechaInicio = r.FechaInicio.ToString("yyyy-MM-dd"),
        FechaFin = r.FechaFin?.ToString("yyyy-MM-dd"),
        SalarioBase = r.SalarioBase,
        Observaciones = r.Observaciones
    };

    public static Contrato ToEntity(this UpdateContractRequest r, int contratoId, int empleadoId) => new()
    {
        Id = contratoId,
        EmpleadoId = empleadoId,
        TipoContratoId = r.TipoContratoId ?? default, // si no se envía, SP mantiene actual
        FechaInicio = r.FechaInicio?.ToString("yyyy-MM-dd"),
        FechaFin = r.FechaFin?.ToString("yyyy-MM-dd"),
        SalarioBase = r.SalarioBase ?? default,
        Observaciones = r.Observaciones
    };

    public static ContractDto ToDto(this Contrato c) => new()
    {
        Id = c.Id,
        EmpleadoId = c.EmpleadoId,
        NombreEmpleado = c.NombreEmpleado,
        TipoContratoId = c.TipoContratoId,
        TipoContratoNombre = c.TipoContratoNombre,
        EstatusContratoId = c.EstatusContratoId,
        EstatusContratoNombre = c.EstatusContratoNombre,
        FechaInicio = c.FechaInicio,
        FechaFin = c.FechaFin,
        SalarioBase = c.SalarioBase,
        Observaciones = c.Observaciones
    };

    public static RenewalDto ToDto(this RenovacionContrato r) => new()
    {
        RenovacionId = r.RenovacionId,
        ContratoId = r.ContratoId,
        FechaRenovacion = r.FechaRenovacion,
        NuevaFechaFin = r.NuevaFechaFin,
        Comentario = r.Comentario,
    };
}
