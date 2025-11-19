namespace Modulo.Asistencias.Presentation.Controllers;

/// <summary>
/// Controlador para la gestión de asistencias de empleados.
/// </summary>
/// <remarks>
/// Proporciona operaciones para registrar entradas/salidas, consultar asistencias
/// y generar reportes mensuales del sistema de control de asistencias.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class AsistenciasController : ControllerBase
{
    private readonly AsistenciasService _svc;

    /// <summary>
    /// Constructor del controlador de asistencias.
    /// </summary>
    /// <param name="svc">Servicio de asistencias inyectado por dependencia.</param>
    public AsistenciasController(AsistenciasService svc) => _svc = svc;

    /// <summary>
    /// Obtiene un listado de asistencias con filtros opcionales.
    /// </summary>
    /// <remarks>
    /// Permite filtrar asistencias por rango de fechas y/o turno específico.
    /// Solo accesible para usuarios con roles de administración o gestión de empleados.
    /// 
    /// Ejemplo de solicitud:
    /// 
    ///     GET /api/asistencias?fechaDesde=2025-01-01&amp;fechaHasta=2025-01-31&amp;turnoId=1
    /// 
    /// </remarks>
    /// <param name="fechaDesde">Fecha inicial del rango de consulta (opcional).</param>
    /// <param name="fechaHasta">Fecha final del rango de consulta (opcional).</param>
    /// <param name="turnoId">Identificador del turno para filtrar (opcional).</param>
    /// <param name="ct">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Lista de asistencias que cumplen con los criterios de búsqueda.</returns>
    /// <response code="200">Retorna la lista de asistencias encontradas.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos suficientes.</response>
    [HttpGet]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(IEnumerable<AsistenciaListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Listar(
        [FromQuery] DateTime? fechaDesde,
        [FromQuery] DateTime? fechaHasta,
        [FromQuery] int? turnoId,
        CancellationToken ct)
    {
        var data = await _svc.ListarAsync(fechaDesde, fechaHasta, turnoId, ct);
        return Ok(data);
    }

    /// <summary>
    /// Registra una marcación de entrada o salida del empleado.
    /// </summary>
    /// <remarks>
    /// Permite al empleado registrar su entrada o salida del lugar de trabajo.
    /// El sistema determina automáticamente si es entrada o salida según el contexto.
    /// Solo accesible para usuarios con rol de empleado.
    /// 
    /// Ejemplo de solicitud:
    /// 
    ///     POST /api/asistencias
    ///     {
    ///         "empleadoId": 123,
    ///         "tipoMarcacion": "Entrada"
    ///     }
    /// 
    /// </remarks>
    /// <param name="req">Datos de la marcación de asistencia.</param>
    /// <param name="ct">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Confirmación de la marcación registrada.</returns>
    /// <response code="201">Marcación registrada exitosamente.</response>
    /// <response code="400">Datos inválidos o regla de negocio violada (ej: ya existe una marcación).</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos suficientes.</response>
    [HttpPost]
    [Authorize(Roles = "empleado")]
    [ProducesResponseType(typeof(AsistenciaInsertResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

    /// <summary>
    /// Actualiza un registro de asistencia existente.
    /// </summary>
    /// <remarks>
    /// Permite modificar los datos de una asistencia previamente registrada.
    /// Solo accesible para usuarios con roles de administración o gestión de empleados.
    /// 
    /// Ejemplo de solicitud:
    /// 
    ///     PUT /api/asistencias/42
    ///     {
    ///         "horaEntrada": "08:00:00",
    ///         "horaSalida": "17:00:00",
    ///         "observaciones": "Ajuste por error en marcación"
    ///     }
    /// 
    /// </remarks>
    /// <param name="id">Identificador único de la asistencia a actualizar.</param>
    /// <param name="req">Datos actualizados de la asistencia.</param>
    /// <param name="ct">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Datos de la asistencia actualizada.</returns>
    /// <response code="200">Asistencia actualizada exitosamente.</response>
    /// <response code="400">Datos inválidos en la solicitud.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos suficientes.</response>
    /// <response code="404">Asistencia no encontrada.</response>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(AsistenciaInsertResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] AsistenciaUpdateRequest req, CancellationToken ct)
    {
        var result = await _svc.ActualizarAsync(id, req, ct);
        return Ok(result);
    }

    /// <summary>
    /// Genera un reporte mensual de asistencias.
    /// </summary>
    /// <remarks>
    /// Produce un reporte consolidado de asistencias para un período específico,
    /// útil para análisis de puntualidad, horas trabajadas y cumplimiento.
    /// Solo accesible para usuarios con roles de administración o gestión de empleados.
    /// 
    /// Ejemplo de solicitud:
    /// 
    ///     GET /api/asistencias/reporte-mensual?fechaInicio=2025-01-01&amp;fechaFin=2025-01-31
    /// 
    /// </remarks>
    /// <param name="fechaInicio">Fecha de inicio del período del reporte.</param>
    /// <param name="fechaFin">Fecha de fin del período del reporte.</param>
    /// <param name="ct">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Reporte mensual de asistencias con estadísticas agregadas.</returns>
    /// <response code="200">Reporte generado exitosamente.</response>
    /// <response code="400">Rango de fechas inválido.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos suficientes.</response>
    [HttpGet("reporte-mensual")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(IEnumerable<AsistenciaReporteMensualDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ReporteMensual([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin, CancellationToken ct)
    {
        var data = await _svc.ReporteMensualAsync(fechaInicio, fechaFin, ct);
        return Ok(data);
    }
}