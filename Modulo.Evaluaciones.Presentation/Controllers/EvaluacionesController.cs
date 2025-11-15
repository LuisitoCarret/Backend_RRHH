using Microsoft.AspNetCore.Http;

namespace Modulo.Evaluaciones.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost]
        [Authorize(Roles = "admin,evaluador")]
        public async Task<IActionResult> Crear([FromBody] CrearEvaluacionRequest request)
        {
            var result = await _crear.HandleAsync(request);

            if (result.Estatus == "error")
                return BadRequest(new { mensaje = result.Mensaje });

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin,evaluador")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarEvaluacionRequest request)
        {
            request.EvaluacionId = id;
            return Ok(await _actualizar.HandleAsync(request));
        }

        [HttpGet]
        [Authorize(Roles = "admin,evaluador")]
        public async Task<IActionResult> Listar([FromQuery] int? area_id, [FromQuery] string? estatus)
            => Ok(await _listar.HandleAsync(area_id, estatus));

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,evaluador")]
        public async Task<IActionResult> Detalle(int id)
        {
            var r = await _detalle.HandleAsync(id);
            return r is null ? NotFound() : Ok(r);
        }

        [HttpGet("me")]
        [Authorize(Roles = "empleado")]
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

        [HttpGet("empleados-disponibles")]
        [Authorize(Roles = "admin,evaluador")]
        public async Task<IActionResult> EmpleadosDisponibles([FromQuery] int plantilla_id)
        {
            var result = await _empleadoDisponible.HandleAsync(plantilla_id);
            return Ok(result);
        }
    }
}
