using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modulo.Empleados.Domain.Entities;

namespace Modulo.Empleados.Domain.Interfaces;

public interface IEmpleadoRepository
{
    Task<IReadOnlyList<Empleado>> GetAllAsync(CancellationToken ct);
    Task<Empleado?> GetByIdAsync(int id, CancellationToken ct);
    Task<(Domicilio? domicilio, ContactoEmergencia? contacto)?> GetMeAsync(long usuarioId, CancellationToken ct);
    Task<(Domicilio? domicilio, ContactoEmergencia? contacto)?> UpdateMeAsync(
        long usuarioId,
        Domicilio domicilio,
        ContactoEmergencia contacto,
        CancellationToken ct);
}

