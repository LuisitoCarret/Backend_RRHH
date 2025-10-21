using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modulo.Empleados.Application.Contracts;
using Modulo.Empleados.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Empleados.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class EmployeesController : ControllerBase
{
    private readonly EmpleadoQueryService _svc;
    public EmployeesController(EmpleadoQueryService svc) => _svc = svc;

    /// <summary>Lista todos los empleados.</summary>
    /// <response code="200">Arreglo de empleados</response>
    [HttpGet]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(IEnumerable<EmployeeListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => Ok(await _svc.ListAsync(ct));

    /// <summary>Obtiene el detalle de un empleado por su ID.</summary>
    /// <param name="id">ID del empleado</param>
    /// <response code="200">Detalle de empleado</response>
    /// <response code="404">No existe el empleado</response>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(EmployeeDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
    {
        var result = await _svc.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Devuelve el perfil (domicilio/contacto) del empleado vinculado al usuario del JWT.</summary>
    /// <response code="200">Perfil del empleado (ME)</response>
    /// <response code="404">El usuario no está vinculado a un empleado</response>
    [HttpGet("me")]
    [Authorize(Roles = "empleado,admin,gestor_empleados")]
    [ProducesResponseType(typeof(MeProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var userId = GetUsuarioId();
        var me = await _svc.GetMeAsync(userId, ct);
        return me is null ? NotFound() : Ok(me);
    }

    /// <summary>Actualiza el perfil (domicilio/contacto) del empleado vinculado al usuario del JWT.</summary>
    /// <response code="200">Perfil actualizado</response>
    /// <response code="404">El usuario no está vinculado a un empleado</response>
    [HttpPut("me")]
    [Authorize(Roles = "empleado,admin,gestor_empleados")]
    [ProducesResponseType(typeof(MeProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateMe([FromBody] MeProfileUpdateRequest request, CancellationToken ct)
    {
        var userId = GetUsuarioId();
        var updated = await _svc.UpdateMeAsync(userId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    private long GetUsuarioId()
    {
        // intenta NameIdentifier, si no, "sub"
        var val = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrWhiteSpace(val))
            throw new InvalidOperationException("El JWT no contiene el identificador de usuario.");
        return long.Parse(val);
    }
}
