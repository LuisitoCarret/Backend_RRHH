namespace Modulo.Empleados.Application.Validators
{
    public class CreateEmpleadoValidator:AbstractValidator<CreateEmpleadoRequest>
    {
        public CreateEmpleadoValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre del empleado es obligatorio.").MaximumLength(120);
            RuleFor(x => x.Email).NotEmpty().WithMessage("El correo electrónico es obligatorio.").EmailAddress();
            RuleFor(x => x.Telefono)
                .Matches(@"^[0-9]{10}$")
                .When(x => !string.IsNullOrEmpty(x.Telefono))
                .WithMessage("El teléfono debe tener 10 dígitos numéricos.");
            RuleFor(x => x.FechaIngreso).NotEmpty();
            RuleFor(x => x.AreaId).GreaterThan(0);
            RuleFor(x => x.PuestoId).GreaterThan(0);
            RuleFor(x => x.TurnoId).GreaterThan(0);
            RuleFor(x => x.EstatusId).GreaterThan(0);

            //Domicilio
            RuleFor(x => x.Domicilio.Calle).NotEmpty();
            RuleFor(x => x.Domicilio.Colonia).NotEmpty();
            RuleFor(x => x.Domicilio.Ciudad).NotEmpty();
            RuleFor(x => x.Domicilio.Estado).NotEmpty();
            RuleFor(x => x.Domicilio.CodigoPostal)
                .Matches(@"^\d{5}$").WithMessage("El código postal debe tener 5 dígitos.");

            //Contacto
            RuleFor(x => x.Contacto.Nombre).NotEmpty();
            RuleFor(x => x.Contacto.Parentesco).NotEmpty();
            RuleFor(x => x.Contacto.Telefono)
                .Matches(@"^[0-9]{10}$").WithMessage("El teléfono del contacto debe tener 10 dígitos.");
        }
    }
}
