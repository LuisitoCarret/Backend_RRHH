using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modulo.Empleados.Application.Contracts;
using Modulo.Empleados.Application.Mappers;
using Modulo.Empleados.Domain.Entities;
using Modulo.Empleados.Domain.Interfaces;

namespace Modulo.Empleados.Application.Services;

public sealed class EmpleadoQueryService
{
    private readonly IEmpleadoRepository _repo;
    public EmpleadoQueryService(IEmpleadoRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<EmployeeListItemDto>> ListAsync(CancellationToken ct)
        => (await _repo.GetAllAsync(ct)).Select(x => x.ToListItemDto()).ToList();

    public async Task<EmployeeDetailDto?> GetByIdAsync(int id, CancellationToken ct)
        => (await _repo.GetByIdAsync(id, ct))?.ToDetailDto();

    public async Task<MeProfileDto?> GetMeAsync(long usuarioId, CancellationToken ct)
    {
        var me = await _repo.GetMeAsync(usuarioId, ct);
        return me is null ? null : me.Value.ToMeDto();
    }

    public async Task<MeProfileDto?> UpdateMeAsync(long usuarioId, MeProfileUpdateRequest req, CancellationToken ct)
    {
        var domicilio = new Domicilio
        {
            Calle = req.Domicilio.Calle,
            Numero = req.Domicilio.Numero,
            Colonia = req.Domicilio.Colonia,
            Ciudad = req.Domicilio.Ciudad,
            Estado = req.Domicilio.Estado,
            CodigoPostal = req.Domicilio.CodigoPostal
        };

        var contacto = new ContactoEmergencia
        {
            Nombre = req.ContactoEmergencia.Nombre,
            Parentesco = req.ContactoEmergencia.Parentesco,
            Telefono = req.ContactoEmergencia.Telefono
        };

        var updated = await _repo.UpdateMeAsync(usuarioId, domicilio, contacto, ct);
        return updated is null ? null : updated.Value.ToMeDto();
    }
}
