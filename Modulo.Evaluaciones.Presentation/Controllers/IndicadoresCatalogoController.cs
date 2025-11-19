using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Modulo.Evaluaciones.Application.Services;
using Modulo.Evaluaciones.Application.Contracts;

namespace Modulo.Evaluaciones.Presentation.Controllers;

/// <summary>
/// Controlador para la consulta del catálogo de indicadores de evaluación.
/// </summary>
/// <remarks>
/// Este catálogo representa la lista base de indicadores disponibles para 
/// construir plantillas de evaluación.
/// 
/// ✔ Es utilizado por el módulo de Plantillas para seleccionar indicadores  
/// ✔ Cada indicador contiene nombre y descripción  
/// ✔ Solo usuarios con rol <b>admin</b> o <b>evaluador</b> pueden consultarlo  
/// 
/// No existen operaciones de creación o edición desde la API; el catálogo
/// se carga desde base de datos.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class IndicadoresCatalogoController : ControllerBase
{
    private readonly IndicadoresCatalogoService _svc;
    public IndicadoresCatalogoController(IndicadoresCatalogoService svc) => _svc = svc;

    // --------------------------------------------------------------------
    // GET /api/IndicadoresCatalogo
    // --------------------------------------------------------------------

    /// <summary>
    /// Obtiene la lista completa de indicadores del catálogo.
    /// </summary>
    /// <remarks>
    /// Esta operación devuelve todos los indicadores base que pueden ser utilizados
    /// para crear plantillas de evaluación.
    ///
    /// Ejemplo de respuesta:
    ///
    /// [
    ///     { "catalogo_id": 1, "nombre": "Puntualidad y asistencia", "descripcion": "Cumplimiento del horario laboral." },
    ///     { "catalogo_id": 2, "nombre": "Trabajo en equipo", "descripcion": "Colaboración y apoyo mutuo." }
    /// ]
    ///
    /// </remarks>
    /// <param name="ct">Token de cancelación.</param>
    /// <returns>Lista de indicadores del catálogo.</returns>
    /// <response code="200">Indicadores obtenidos correctamente.</response>
    /// <response code="400">Error de consulta o excepción SQL.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos.</response>
    /// <response code="500">Error interno no controlado.</response>
    [HttpGet]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(IEnumerable<IndicadoresCatalogoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        try
        {
            var data = await _svc.GetAllAsync(ct);
            return Ok(data);
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
