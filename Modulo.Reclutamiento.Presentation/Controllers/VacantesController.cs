// ==========================
// Modulo.Reclutamiento.Presentation/Controllers/VacantesController.cs
// ==========================
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Modulo.Reclutamiento.Application.Common;
using Modulo.Reclutamiento.Application.Contracts;
using Modulo.Reclutamiento.Application.Services;
using System.Runtime.InteropServices;

namespace Modulo.Reclutamiento.Presentation.Controllers;

[ApiController]
[Route("api/reclutamiento/vacantes")]
[Produces("application/json")]
public sealed class VacantesController : ControllerBase
{
    private readonly VacantesService _svc;
    public VacantesController(VacantesService svc) => _svc = svc;

    [HttpPost]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(VacanteDetailDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateVacanteRequest req, CancellationToken ct)
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
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpGet]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(ListVacantesResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] string? estatus, [FromQuery] int? areaId, [FromQuery] int? puestoId,
                                          [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        try
        {
            var dto = await _svc.ListAsync(new ListVacantesRequest
            {
                Estatus = estatus,
                AreaId = areaId,
                PuestoId = puestoId,
                Page = page,
                PageSize = pageSize
            }, ct);

            return Ok(dto);
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
        catch (Exception ex)
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
