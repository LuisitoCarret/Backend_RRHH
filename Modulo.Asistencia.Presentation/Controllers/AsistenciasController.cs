using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modulo.Asistencias.Application.Common;
using Modulo.Asistencias.Application.Contracts;
using Modulo.Asistencias.Application.Services;

namespace Modulo.Asistencias.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AsistenciasController : ControllerBase
{
    private readonly AsistenciasService _svc;
    public AsistenciasController(AsistenciasService svc) => _svc = svc;

    /// <summary>Lista asistencias con filtros por fecha/turno.</summary>
    [HttpGet]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(IEnumerable<AsistenciaListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] int? turnoId,
        CancellationToken ct)
    {
        var data = await _svc.ListarAsync(fechaDesde, fechaHasta, turnoId, ct);
        return Ok(data);
    }

    /// <summary>Marca Entrada o Salida del empleado.</summary>
    [HttpPost]
    [Authorize(Roles = "empleado")]
    [ProducesResponseType(typeof(AsistenciaInsertResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Insertar([FromBody] AsistenciaInsertRequest req, CancellationToken ct)
    {
        try
        {
            var res = await _svc.InsertarAsync(req, ct);
            return StatusCode(StatusCodes.Status201Created, res);
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
    }
}
