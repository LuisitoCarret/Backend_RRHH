namespace Modulo.Reclutamiento.Presentation.Controllers;

/// <summary>
/// Controlador encargado de gestionar vacantes y postulaciones dentro del módulo de Reclutamiento.
/// </summary>
/// <remarks>
/// Este módulo permite:
/// 
/// ✔ Crear, listar, consultar y actualizar vacantes  
/// ✔ Registrar postulaciones a una vacante  
/// ✔ Consultar y actualizar postulaciones  
/// 
/// Todos los endpoints requieren permisos de administrador o reclutador.
/// </remarks>
[ApiController]
[Route("api/reclutamiento/vacantes")]
[Produces("application/json")]
public sealed class VacantesController : ControllerBase
{
    private readonly VacantesService _svc;
    private readonly PostulacionesService _postSvc;

    public VacantesController(VacantesService svc, PostulacionesService postSvc)
    {
        _svc = svc;
        _postSvc = postSvc;
    }

    // ============================================================
    // POST: Crear vacante
    // ============================================================

    /// <summary>
    /// Crea una nueva vacante.
    /// </summary>
    /// <remarks>
    /// Ejemplo:
    /// 
    ///     POST /api/reclutamiento/vacantes
    ///     {
    ///         "titulo": "Desarrollador Backend",
    ///         "descripcion": "Experiencia en .NET",
    ///         "areaId": 2,
    ///         "puestoId": 5,
    ///         "fechaPublicacion": "2025-01-01"
    ///     }
    /// </remarks>
    /// <response code="201">Vacante creada correctamente.</response>
    /// <response code="400">Datos inválidos o regla de negocio violada.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos.</response>
    [HttpPost]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(VacanteDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                Title = "Regla de negocio",
                Detail = ex.Message
            });
        }
    }

    // ============================================================
    // GET: Lista de vacantes
    // ============================================================

    /// <summary>
    /// Lista vacantes con filtros y paginación.
    /// </summary>
    /// <response code="200">Lista obtenida correctamente.</response>
    /// <response code="400">Error en filtros o parámetros.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpGet]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(ListVacantesResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> List(
        [FromQuery] string? estatus,
        [FromQuery] int? areaId,
        [FromQuery] int? puestoId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _svc.ListAsync(
                new ListVacantesRequest
                {
                    Estatus = estatus,
                    AreaId = areaId,
                    PuestoId = puestoId,
                    Page = page,
                    PageSize = pageSize
                }, ct);

            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails { Detail = ex.Message });
        }
    }

    // ============================================================
    // GET: Detalle de vacante
    // ============================================================

    /// <summary>
    /// Obtiene el detalle completo de una vacante.
    /// </summary>
    /// <response code="200">Detalle obtenido correctamente.</response>
    /// <response code="400">Regla de negocio violada.</response>
    /// <response code="404">Vacante no encontrada.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(VacanteDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetDetalle(int id, CancellationToken ct)
    {
        try
        {
            var result = await _svc.GetDetalleAsync(id, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ============================================================
    // PUT: Actualizar vacante
    // ============================================================

    /// <summary>
    /// Actualiza los datos generales de una vacante (sin modificar estatus).
    /// </summary>
    /// <response code="200">Vacante actualizada correctamente.</response>
    /// <response code="400">Error en reglas de negocio.</response>
    /// <response code="404">Vacante no encontrada.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(VacanteDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVacanteRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _svc.UpdateAsync(id, req, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ============================================================
    // POST: Crear postulación
    // ============================================================

    /// <summary>
    /// Crea una postulación asociada a una vacante.
    /// </summary>
    /// <remarks>Soporta carga de archivo CV (multipart/form-data).</remarks>
    /// <response code="200">Postulación creada.</response>
    /// <response code="400">Error de negocio.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpPost("postulaciones")]
    [Authorize(Roles = "admin,reclutador")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(PostulacionDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreatePostulacion([FromForm] CreatePostulacionRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _postSvc.CreateAsync(req, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ============================================================
    // GET: Listado de postulaciones
    // ============================================================

    /// <summary>
    /// Lista las postulaciones de candidatos con filtros y paginación.
    /// </summary>
    /// <response code="200">Listado generado.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpGet("postulaciones")]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(PostulacionListResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ListPostulaciones(
        [FromQuery] string? vacanteNombre,
        [FromQuery] string? estatus,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        try
        {
            var result = await _postSvc.ListAsync(vacanteNombre, estatus, page, pageSize, ct);
            return Ok(result);
        }
        catch
        {
            return StatusCode(500, new { success = false, message = "Error al listar las postulaciones." });
        }
    }

    // ============================================================
    // GET: Detalle de postulación
    // ============================================================

    /// <summary>
    /// Obtiene el detalle completo de una postulación.
    /// </summary>
    /// <response code="200">Detalle obtenido.</response>
    /// <response code="400">Error de negocio.</response>
    /// <response code="404">Postulación no encontrada.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpGet("postulaciones/{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(PostulacionDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetPostulacionDetalle(int id, CancellationToken ct)
    {
        try
        {
            var result = await _postSvc.GetDetalleAsync(id, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // ============================================================
    // PUT: Actualizar postulación
    // ============================================================

    /// <summary>
    /// Actualiza el estatus de una postulación.
    /// Si se marca como <b>aceptada</b>, la vacante se cierra automáticamente.
    /// </summary>
    /// <response code="200">Postulación actualizada.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpPut("postulaciones/{id:int}")]
    [Authorize(Roles = "admin,reclutador")]
    [ProducesResponseType(typeof(PostulacionUpdateResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdatePostulacion(int id, [FromBody] UpdatePostulacionRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _postSvc.UpdateAsync(id, req, ct);
            return Ok(result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}
