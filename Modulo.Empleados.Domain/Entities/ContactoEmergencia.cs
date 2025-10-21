using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Empleados.Domain.Entities;

public sealed class ContactoEmergencia
{
    public string Nombre { get; init; } = default!;
    public string Parentesco { get; init; } = default!;
    public string Telefono { get; init; } = default!;
}
