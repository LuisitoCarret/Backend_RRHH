using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentValidation;
using Modulo.Contratos.Application.Contracts;

namespace Modulo.Contratos.Application.Validators;

public sealed class UpdateContractValidator : AbstractValidator<UpdateContractRequest>
{
    public UpdateContractValidator()
    {
        RuleFor(x => x.TipoContratoId).GreaterThan(0);
        RuleFor(x => x.EstatusContratoId).GreaterThan(0);
        RuleFor(x => x.FechaInicio).NotEmpty();
        RuleFor(x => x.SalarioBase).GreaterThan(0);
        RuleFor(x => x).Must(x =>
            !x.FechaFin.HasValue || x.FechaFin.Value.Date > x.FechaInicio.Date
        ).WithMessage("fechaFin debe ser mayor a fechaInicio cuando se envíe.");
    }
}
