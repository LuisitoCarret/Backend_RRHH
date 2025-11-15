// ==========================
// Modulo.Reclutamiento.Presentation/Controllers/VacantesController.cs
// ==========================
namespace Modulo.Reclutamiento.Presentation.Controllers;

[ApiController]
[Route("api/reclutamiento/vacantes")]
[Produces("application/json")]
public sealed class VacantesController : ControllerBase
{
    private readonly VacantesService _svc;
    private readonly PostulacionesService _service;
    public VacantesController(VacantesService svc, PostulacionesService service)
    {
        _svc = svc;
        _service = service; 
    }

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

    [HttpGet("{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    public async Task<IActionResult> GetDetalle(int id, CancellationToken ct)
    {
        try
        {
            var result = await _svc.GetDetalleAsync(id, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Ocurrió un error al obtener la información de la vacante."
            });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVacanteRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _svc.UpdateAsync(id, req, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Ocurrió un error al actualizar la vacante."
            });
        }
    }

    [HttpPost("postulaciones")]
    [Authorize(Roles = "admin,reclutador")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreatePostulacion([FromForm] CreatePostulacionRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _service.CreateAsync(req, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { success = false, message = "Ocurrió un error al registrar la postulación." });
        }
    }

    [HttpGet("postulaciones")]
    [Authorize(Roles = "admin,reclutador")]
    public async Task<IActionResult> List(
     [FromQuery] string? vacanteNombre,
     [FromQuery] string? estatus,
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 10,
     CancellationToken ct = default)
    {
        try
        {
            var result = await _service.ListAsync(vacanteNombre, estatus, page, pageSize, ct);
            return Ok(result);
        }
        catch
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Ocurrió un error al listar las postulaciones."
            });
        }
    }

    [HttpGet("postulaciones/{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    public async Task<IActionResult> GetDetallePostulaciones(int id, CancellationToken ct)
    {
        try
        {
            var result = await _service.GetDetalleAsync(id, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Ocurrió un error al obtener la información de la postulación."
            });
        }
    }

    [HttpPut("postulaciones/{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePostulacionRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _service.UpdateAsync(id, req, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
        catch
        {
            return StatusCode(500, new
            {
                success = false,
                message = "Ocurrió un error al actualizar la postulación."
            });
        }
    }

}
