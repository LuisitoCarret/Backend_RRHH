using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Empleados.Domain.Entities;

public sealed class Empleado
{
    public int Id { get; init; }
    public string Nombre { get; init; } = default!;
    public string Correo { get; init; } = default!;
    public string Telefono { get; init; } = default!;
    public string FechaIngreso { get; init; } = default!; // yyyy-MM-dd
    public string Area { get; init; } = default!;
    public string Puesto { get; init; } = default!;
    public string Turno { get; init; } = default!;
    public string Estatus { get; init; } = default!;
    public Domicilio? Domicilio { get; init; }
    public ContactoEmergencia? Contacto { get; init; }
}
