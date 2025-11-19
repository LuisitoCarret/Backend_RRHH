using Microsoft.AspNetCore.Http;
using Modulo.Evaluaciones.Domain.Entities;

namespace Modulo.Evaluaciones.Presentation.Controllers
{
    /// <summary>
    /// Controlador para la gestión del módulo de evaluaciones de desempeño.
    /// </summary>
    /// <remarks>
    /// Permite crear evaluaciones, registrar avances, cerrar evaluaciones,
    /// consultar listados generales, obtener el detalle completo y consultar
    /// el historial del empleado autenticado.
    ///
    /// Flujo funcional:
    /// 1. Se crea una evaluación seleccionando empleado y plantilla.
    /// 2. Se registran calificaciones parciales (estatus en_proceso).
    /// 3. Al cerrar, el sistema valida que todas las calificaciones estén completas,
    ///    calcula el puntaje total y determina el nivel de desempeño.
    /// 4. Una evaluación cerrada ya no puede modificarse.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EvaluacionesController : ControllerBase
    {
        private readonly CrearEvaluacionService _crear;
        private readonly ActualizarEvaluacionService _actualizar;
        private readonly ListarEvaluacionesService _listar;
        private readonly ObtenerDetalleEvaluacionService _detalle;
        private readonly ObtenerMisEvaluacionesService _misEvaluaciones;
        private readonly ObtenerEmpleadoDisponibleService _empleadoDisponible;

        public EvaluacionesController(
            CrearEvaluacionService crear,
            ActualizarEvaluacionService actualizar,
            ListarEvaluacionesService listar,
            ObtenerDetalleEvaluacionService detalle,
            ObtenerMisEvaluacionesService misEvaluaciones,
            ObtenerEmpleadoDisponibleService empleadoDisponible)
        {
            _crear = crear;
            _actualizar = actualizar;
            _listar = listar;
            _detalle = detalle;
            _misEvaluaciones = misEvaluaciones;
            _empleadoDisponible = empleadoDisponible;
        }

        // --------------------------------------------------------------------
        // POST /api/evaluaciones
        // --------------------------------------------------------------------

        /// <summary>
        /// Crea una nueva evaluación para un empleado utilizando una plantilla vigente.
        /// </summary>
        /// <remarks>
        /// Valida:
        /// - que la plantilla exista y esté vigente
        /// - que el empleado pertenezca al área de la plantilla
        /// 
        /// La evaluación se crea con estatus <b>en_proceso</b>.
        ///
        /// Ejemplo de solicitud:
        /// 
        ///     POST /api/evaluaciones
        ///     {
        ///         "empleado_id": 15,
        ///         "plantilla_id": 3
        ///     }
        /// 
        /// Ejemplo de respuesta:
        /// 
        ///     {
        ///         "evaluacionId": 25,
        ///         "estatus": "en_proceso",
        ///         "mensaje": "Evaluación creada correctamente"
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Datos para la creación de la evaluación.</param>
        /// <returns>Datos de la evaluación creada.</returns>
        /// <response code="200">Evaluación creada correctamente.</response>
        /// <response code="400">La plantilla o el empleado no cumplen las validaciones.</response>
        /// <response code="401">Usuario no autenticado.</response>
        /// <response code="403">Usuario sin permisos.</response>
        [HttpPost]
        [Authorize(Roles = "admin,evaluador")]
        [ProducesResponseType(typeof(CrearEvaluacionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Crear([FromBody] CrearEvaluacionRequest request)
        {
            var result = await _crear.HandleAsync(request);

            if (result.Estatus == "error")
                return BadRequest(new { mensaje = result.Mensaje });

            return Ok(result);
        }

        // --------------------------------------------------------------------
        // PUT /api/evaluaciones/{id}
        // --------------------------------------------------------------------

        /// <summary>
        /// Actualiza una evaluación existente (guardar avances o cerrar).
        /// </summary>
        /// <remarks>
        /// Comportamiento:
        /// - <b>en_proceso:</b> Guarda avances parciales de calificación.
        /// - <b>cerrada:</b> Requiere todas las calificaciones y retroalimentación.
        ///   Se calcula puntaje total y nivel de desempeño.
        /// 
        /// Ejemplo de guardar avances:
        /// 
        ///     PUT /api/evaluaciones/25
        ///     {
        ///         "estatus": "en_proceso",
        ///         "retroalimentacion": "Aún evaluando...",
        ///         "detalle": [
        ///             { "indicador_id": 101, "calificacion": 8 },
        ///             { "indicador_id": 102, "calificacion": null }
        ///         ]
        ///     }
        /// 
        /// Ejemplo de cierre:
        /// 
        ///     PUT /api/evaluaciones/25
        ///     {
        ///         "estatus": "cerrada",
        ///         "retroalimentacion": "Desempeño destacado.",
        ///         "detalle": [
        ///             { "indicador_id": 101, "calificacion": 9 },
        ///             { "indicador_id": 102, "calificacion": 8 }
        ///         ]
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">ID de la evaluación a actualizar.</param>
        /// <param name="request">Datos actualizados.</param>
        /// <returns>Evaluación actualizada.</returns>
        /// <response code="200">Actualización exitosa.</response>
        /// <response code="400">Datos inválidos o falta calificación al cerrar.</response>
        /// <response code="404">La evaluación no existe.</response>
        /// <response code="401">Usuario no autenticado.</response>
        /// <response code="403">Usuario sin permisos.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,evaluador")]
        [ProducesResponseType(typeof(ActualizarEvaluacionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEvaluacionRequest request)
        {
            request.EvaluacionId = id;
            var result = await _actualizar.HandleAsync(request);
            return Ok(result);
        }

        // --------------------------------------------------------------------
        // GET /api/evaluaciones
        // --------------------------------------------------------------------

        /// <summary>
        /// Lista evaluaciones con filtros opcionales (área y/o estatus).
        /// </summary>
        /// <remarks>
        /// Permite obtener un listado general filtrado por:
        /// - área del empleado
        /// - estatus de la evaluación
        ///
        /// Ejemplo:
        ///     GET /api/evaluaciones?area_id=2&amp;estatus=cerrada
        /// </remarks>
        /// <param name="area_id">ID del área (opcional).</param>
        /// <param name="estatus">Estatus de la evaluación (opcional).</param>
        /// <returns>Lista de evaluaciones.</returns>
        /// <response code="200">Listado generado correctamente.</response>
        [HttpGet]
        [Authorize(Roles = "admin,evaluador")]
        [ProducesResponseType(typeof(IEnumerable<ListarEvaluacionResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Listar([FromQuery] int? area_id, [FromQuery] string? estatus)
            => Ok(await _listar.HandleAsync(area_id, estatus));

        // --------------------------------------------------------------------
        // GET /api/evaluaciones/{id}
        // --------------------------------------------------------------------

        /// <summary>
        /// Obtiene el detalle completo de una evaluación.
        /// </summary>
        /// <remarks>
        /// Incluye:
        /// - empleado evaluado
        /// - plantilla utilizada
        /// - puntaje total y nivel de desempeño
        /// - retroalimentación general
        /// - indicadores con nombre, ponderación y calificación
        ///
        /// Ejemplo:
        ///     GET /api/evaluaciones/25
        /// </remarks>
        /// <param name="id">ID de la evaluación.</param>
        /// <returns>Detalle completo.</returns>
        /// <response code="200">Detalle obtenido.</response>
        /// <response code="404">Evaluación no encontrada.</response>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,evaluador")]
        [ProducesResponseType(typeof(DetalleEvaluacionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Detalle(int id)
        {
            var r = await _detalle.HandleAsync(id);
            return r is null ? NotFound() : Ok(r);
        }

        // --------------------------------------------------------------------
        // GET /api/evaluaciones/me
        // --------------------------------------------------------------------

        /// <summary>
        /// Obtiene el historial de evaluaciones del empleado autenticado.
        /// </summary>
        /// <remarks>
        /// Requiere que el token contenga <b>empleado_id</b>.
        ///
        /// Ejemplo:
        ///     GET /api/evaluaciones/me
        /// </remarks>
        /// <returns>Lista de evaluaciones del empleado.</returns>
        /// <response code="200">Historial obtenido.</response>
        /// <response code="401">El token no contiene empleado_id.</response>
        [HttpGet("me")]
        [Authorize(Roles = "empleado")]
        [ProducesResponseType(typeof(IEnumerable<ListarEvaluacionResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MisEvaluaciones()
        {
            var empleadoId = ObtenerEmpleadoIdDelToken();

            if (empleadoId is null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized,
                    new { mensaje = "El token no contiene empleado_id." });
            }

            var result = await _misEvaluaciones.HandleAsync(empleadoId.Value);
            return Ok(result);
        }

        private int? ObtenerEmpleadoIdDelToken()
        {
            var value = User.FindFirst("empleado_id")?.Value;
            return int.TryParse(value, out var id) ? id : null;
        }

        // --------------------------------------------------------------------
        // GET /api/evaluaciones/empleados-disponibles
        // --------------------------------------------------------------------

        /// <summary>
        /// Obtiene empleados aptos para iniciar una evaluación según la plantilla seleccionada.
        /// </summary>
        /// <remarks>
        /// Filtra empleados que:
        /// - pertenecen al área de la plantilla
        /// - no tienen una evaluación abierta con esa plantilla
        ///
        /// Ejemplo:
        ///     GET /api/evaluaciones/empleados-disponibles?plantilla_id=3
        /// </remarks>
        /// <param name="plantilla_id">ID de la plantilla.</param>
        /// <returns>Lista de empleados disponibles.</returns>
        /// <response code="200">Lista generada correctamente.</response>
        [HttpGet("empleados-disponibles")]
        [Authorize(Roles = "admin,evaluador")]
        [ProducesResponseType(typeof(IEnumerable<EmpleadoDisponibleResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EmpleadosDisponibles([FromQuery] int plantilla_id)
        {
            var result = await _empleadoDisponible.HandleAsync(plantilla_id);
            return Ok(result);
        }
    }
}
