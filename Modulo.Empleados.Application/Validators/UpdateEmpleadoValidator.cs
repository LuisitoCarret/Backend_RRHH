namespace Modulo.Empleados.Application.Validators
{
    public class UpdateEmpleadoValidator:AbstractValidator<UpdateEmpleadoRequest>
    {
        public UpdateEmpleadoValidator()
        {
            RuleFor(x => x.Nombre)
           .MaximumLength(120).When(x => !string.IsNullOrEmpty(x.Nombre));

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Telefono)
                .Matches(@"^[0-9]{10}$")
                .When(x => !string.IsNullOrEmpty(x.Telefono))
                .WithMessage("El teléfono debe tener 10 dígitos.");

            RuleFor(x => x.AreaId).GreaterThan(0);
            RuleFor(x => x.PuestoId).GreaterThan(0);
            RuleFor(x => x.TurnoId).GreaterThan(0);
            RuleFor(x => x.EstatusId).GreaterThan(0);
        }
    }
}
