namespace Modulo.Contratos.Application.Validators;

public sealed class UpdateContractValidator : AbstractValidator<UpdateContractRequest>
{
    public UpdateContractValidator()
    {
        // Tipo de contrato opcional pero válido si se envía
        When(x => x.TipoContratoId.HasValue, () =>
        {
            RuleFor(x => x.TipoContratoId)
                .GreaterThan(0)
                .WithMessage("Debe especificar un tipo de contrato válido.");
        });

        // Validar salario si se envía
        When(x => x.SalarioBase.HasValue, () =>
        {
            RuleFor(x => x.SalarioBase)
                .GreaterThan(0)
                .WithMessage("El salario base debe ser mayor a 0.");
        });

        // Validar fechas si ambas se envían
        When(x => x.FechaInicio.HasValue && x.FechaFin.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.FechaFin > x.FechaInicio)
                .WithMessage("La fecha de fin debe ser mayor a la fecha de inicio.");
        });
    }
}