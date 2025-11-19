using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Modulo.Evaluaciones.Application.Contracts;
using Modulo.Evaluaciones.Application.Services;

namespace Modulo.Evaluaciones.Presentation.Controllers;

/// <summary>
/// Controlador para la administración de plantillas de evaluación.
/// </summary>
/// <remarks>
/// Este módulo permite:
/// 
/// ✔ Listar plantillas con filtros por área y vigencia  
/// ✔ Consultar el detalle completo de una plantilla  
/// ✔ Crear nuevas plantillas con indicadores y ponderaciones  
/// ✔ Actualizar la vigencia (activar/desactivar)  
/// 
/// Las plantillas definen los indicadores que serán evaluados y se asocian
/// a un área específica, así como a un periodo de validez.
/// Son utilizadas posteriormente por el módulo de Evaluaciones
/// para generar evaluaciones de desempeño.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class PlantillasController : ControllerBase
{
    private readonly PlantillasQueryService _query;
    private readonly PlantillasCommandService _cmd;

    public PlantillasController(PlantillasQueryService query, PlantillasCommandService cmd)
    {
        _query = query;
        _cmd = cmd;
    }

    // --------------------------------------------------------------------
    // GET /api/Plantillas
    // --------------------------------------------------------------------

    /// <summary>
    /// Lista plantillas de evaluación con filtros opcionales.
    /// </summary>
    /// <remarks>
    /// Permite filtrar por:
    /// - área a la que pertenece la plantilla  
    /// - estado de vigencia (vigente / no vigente)  
    /// 
    /// Ejemplo de solicitud:
    ///
    ///     GET /api/plantillas?area_id=2&amp;vigente=true
    ///
    /// Ejemplo de respuesta:
    /// [
    ///     {
    ///         "plantilla_id": 3,
    ///         "nombre": "Evaluación Semestral",
    ///         "area_id": 2,
    ///         "nombre_area": "Ventas",
    ///         "periodo_inicio": "2025-01-01",
    ///         "periodo_fin": "2025-06-30",
    ///         "vigente": true
    ///     }
    /// ]
    /// </remarks>
    /// <param name="area_id">Área para filtrar (opcional).</param>
    /// <param name="vigente">Estado de vigencia (opcional).</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Listado de plantillas.</returns>
    /// <response code="200">Listado generado correctamente.</response>
    /// <response code="400">Error de consulta o parámetros inválidos.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos.</response>
    [HttpGet]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(List<PlantillaListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> List([FromQuery] int? area_id, [FromQuery] bool? vigente, CancellationToken ct)
    {
        try
        {
            var list = await _query.ListAsync(area_id, vigente, ct);
            return Ok(list);
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

    // --------------------------------------------------------------------
    // GET /api/Plantillas/{id}
    // --------------------------------------------------------------------

    /// <summary>
    /// Obtiene el detalle completo de una plantilla de evaluación.
    /// </summary>
    /// <remarks>
    /// Incluye:
    /// - Información general de la plantilla  
    /// - Área y periodo de vigencia  
    /// - Lista de indicadores con sus ponderaciones  
    ///
    /// Ejemplo:
    /// 
    ///     GET /api/plantillas/5
    /// 
    /// Ejemplo de respuesta:
    ///
    ///     {
    ///         "plantilla_id": 5,
    ///         "nombre": "Eval. Anual RRHH",
    ///         "area_id": 1,
    ///         "nombre_area": "Recursos Humanos",
    ///         "periodo_inicio": "2025-01-01",
    ///         "periodo_fin": "2025-12-31",
    ///         "vigente": true,
    ///         "indicadores": [
    ///             { "indicador_id": 101, "catalogo_id": 1, "nombre": "Trabajo en equipo", "ponderacion": 25 }
    ///         ]
    ///     }
    /// </remarks>
    /// <param name="id">ID de la plantilla.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Detalle de la plantilla.</returns>
    /// <response code="200">Detalle obtenido correctamente.</response>
    /// <response code="404">Plantilla no encontrada.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos.</response>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(PlantillaDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Detail([FromRoute] int id, CancellationToken ct)
    {
        try
        {
            var d = await _query.GetAsync(id, ct);
            return d is null ? NotFound() : Ok(d);
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

    // --------------------------------------------------------------------
    // POST /api/Plantillas
    // --------------------------------------------------------------------

    /// <summary>
    /// Crea una nueva plantilla de evaluación.
    /// </summary>
    /// <remarks>
    /// Una plantilla incluye:
    /// - Nombre  
    /// - Área asignada  
    /// - Periodo de vigencia  
    /// - Lista de indicadores con ponderaciones sumando 100%  
    /// 
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/plantillas
    ///     {
    ///         "nombre": "Evaluación Semestral TI",
    ///         "descripcion": "Plantilla base para el área de TI",
    ///         "area_id": 4,
    ///         "periodo_inicio": "2025-01-01",
    ///         "periodo_fin": "2025-06-30",
    ///         "indicadores": [
    ///             { "catalogo_id": 1, "ponderacion": 20 },
    ///             { "catalogo_id": 5, "ponderacion": 30 }
    ///         ]
    ///     }
    ///
    /// Ejemplo de respuesta:
    ///
    ///     {
    ///         "plantilla_id": 10,
    ///         "mensaje": "Plantilla creada correctamente"
    ///     }
    ///
    /// </remarks>
    /// <param name="req">Datos de creación.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Datos de la plantilla creada.</returns>
    /// <response code="201">Plantilla creada correctamente.</response>
    /// <response code="400">Datos inválidos o reglas de negocio incumplidas.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos.</response>
    /// <response code="500">Error inesperado del servidor.</response>
    [HttpPost]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(CreatePlantillaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreatePlantillaRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _cmd.CreateAsync(req, ct);
            return StatusCode(StatusCodes.Status201Created, result);
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

    // --------------------------------------------------------------------
    // PUT /api/Plantillas/{id}/Vigencia
    // --------------------------------------------------------------------

    /// <summary>
    /// Actualiza la vigencia de una plantilla (activar o desactivar).
    /// </summary>
    /// <remarks>
    /// Esta operación permite controlar si una plantilla
    /// puede seguir siendo utilizada para generar nuevas evaluaciones.
    /// 
    /// Ejemplo:
    ///
    ///     PUT /api/plantillas/10/vigencia
    ///     {
    ///         "vigente": false
    ///     }
    ///
    /// Ejemplo de respuesta:
    ///
    ///     {
    ///         "mensaje": "Vigencia actualizada correctamente"
    ///     }
    /// </remarks>
    /// <param name="id">ID de la plantilla.</param>
    /// <param name="req">Estado de vigencia a aplicar.</param>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Mensaje indicando el resultado de la operación.</returns>
    /// <response code="200">Vigencia actualizada correctamente.</response>
    /// <response code="400">Error al actualizar o datos inválidos.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPut("{id:int}/Vigencia")]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(UpdateVigenciaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVigencia([FromRoute] int id, [FromBody] UpdateVigenciaRequest req, CancellationToken ct)
    {
        try
        {
            var resp = await _cmd.UpdateVigenciaAsync(id, req.vigente, ct);
            return Ok(resp);
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
