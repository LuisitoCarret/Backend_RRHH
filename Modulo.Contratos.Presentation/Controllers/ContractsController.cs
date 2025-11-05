using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Threading;
using Modulo.Contratos.Application.Common;   // BusinessRuleException
using Modulo.Contratos.Application.Contracts;
using Modulo.Contratos.Application.Services;

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
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest req, CancellationToken ct)
    {
        try
        {
            var created = await _svc.CreateAsync(req, ct);
            return StatusCode(StatusCodes.Status201Created, created);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    /// <summary>Actualiza un contrato existente.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateContractRequest req, [FromQuery] int empleadoId, CancellationToken ct)
    {
        try
        {
            var updated = await _svc.UpdateAsync(id, empleadoId, req, ct);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    /// <summary>Elimina lógicamente un contrato.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        try
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    /// <summary>Crea una renovación para un contrato y actualiza su fecha de fin.</summary>
    [HttpPost("{id:int}/renewals")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(RenewalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Renew([FromRoute] int id, [FromBody] CreateRenewalRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _svc.RenewAsync(id, req, ct);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Bad Request",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (System.Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
