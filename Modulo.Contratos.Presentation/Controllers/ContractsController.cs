using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modulo.Contratos.Application.Contracts;
using Modulo.Contratos.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Contratos.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class ContractsController : ControllerBase
{
    private readonly ContratoCommandService _svc;
    public ContractsController(ContratoCommandService svc) => _svc = svc;

    /// <summary>Crea un nuevo contrato.</summary>
    [HttpPost]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest req, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(req, ct);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    /// <summary>Actualiza un contrato existente.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateContractRequest req, [FromQuery] int empleadoId, CancellationToken ct)
    {
        // empleadoId se conserva por claridad de modelo; no se permite cambiar en SP
        var updated = await _svc.UpdateAsync(id, empleadoId, req, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Elimina lógicamente un contrato.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        var ok = await _svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>Crea una renovación para un contrato y actualiza su fecha de fin.</summary>
    [HttpPost("{id:int}/renewals")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(RenewalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Renew([FromRoute] int id, [FromBody] CreateRenewalRequest req, CancellationToken ct)
    {
        var result = await _svc.RenewAsync(id, req, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }
}
