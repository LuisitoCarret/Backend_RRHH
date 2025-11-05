using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modulo.Contratos.Application.Contracts;
using Modulo.Contratos.Domain.Entities;

namespace Modulo.Contratos.Application.Mapping;

public static class ContractMappers
{
    public static Contrato ToEntity(this CreateContractRequest r) => new()
    {
        EmpleadoId = r.EmpleadoId,
        TipoContratoId = r.TipoContratoId,
        EstatusContratoId = r.EstatusContratoId,
        FechaInicio = r.FechaInicio.ToString("yyyy-MM-dd"),
        FechaFin = r.FechaFin?.ToString("yyyy-MM-dd"),
        SalarioBase = r.SalarioBase,
        Observaciones = r.Observaciones
    };

    public static Contrato ToEntity(this UpdateContractRequest r, int contratoId, int empleadoId) => new()
    {
        Id = contratoId,
        EmpleadoId = empleadoId,
        TipoContratoId = r.TipoContratoId,
        EstatusContratoId = r.EstatusContratoId,
        FechaInicio = r.FechaInicio.ToString("yyyy-MM-dd"),
        FechaFin = r.FechaFin?.ToString("yyyy-MM-dd"),
        SalarioBase = r.SalarioBase,
        Observaciones = r.Observaciones
    };

    public static ContractDto ToDto(this Contrato c) => new()
    {
        Id = c.Id,
        EmpleadoId = c.EmpleadoId,
        TipoContratoId = c.TipoContratoId,
        EstatusContratoId = c.EstatusContratoId,
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
        Comentario = r.Comentario
    };
}
