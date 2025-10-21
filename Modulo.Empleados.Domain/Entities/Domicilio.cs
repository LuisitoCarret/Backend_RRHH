using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Empleados.Domain.Entities;

public sealed class Domicilio
{
    public string Calle { get; init; } = default!;
    public string Numero { get; init; } = default!;
    public string Colonia { get; init; } = default!;
    public string Ciudad { get; init; } = default!;
    public string Estado { get; init; } = default!;
    public string CodigoPostal { get; init; } = default!;
}

