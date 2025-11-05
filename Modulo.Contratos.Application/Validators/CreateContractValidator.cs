namespace Modulo.Contratos.Application.Validators;

public sealed class CreateContractValidator : AbstractValidator<CreateContractRequest>
{
    public CreateContractValidator()
    {
        RuleFor(x => x.EmpleadoId)
            .GreaterThan(0)
            .WithMessage("Debe especificar un empleado válido.");

        RuleFor(x => x.TipoContratoId)
            .GreaterThan(0)
            .WithMessage("Debe seleccionar un tipo de contrato válido.");

        RuleFor(x => x.FechaInicio)
            .NotEmpty()
            .WithMessage("Debe especificar la fecha de inicio del contrato.");

        RuleFor(x => x.SalarioBase)
            .GreaterThan(0)
            .WithMessage("El salario base debe ser mayor a 0.");

        // Validar coherencia de fechas (fechaFin > fechaInicio si aplica)
        When(x => x.FechaFin.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.FechaFin > x.FechaInicio)
                .WithMessage("La fecha de fin debe ser mayor a la fecha de inicio.");
        });
    }
}
