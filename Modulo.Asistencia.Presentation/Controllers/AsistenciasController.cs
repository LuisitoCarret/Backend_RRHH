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

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(AsistenciaInsertResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] AsistenciaUpdateRequest req, CancellationToken ct)
    {
        var result = await _svc.ActualizarAsync(id, req, ct);
        return Ok(result);
    }

    [HttpGet("reporte-mensual")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(IEnumerable<AsistenciaReporteMensualDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReporteMensual([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, CancellationToken ct)
    {
        var data = await _svc.ReporteMensualAsync(fechaInicio, fechaFin, ct);
        return Ok(data);
    }


}
