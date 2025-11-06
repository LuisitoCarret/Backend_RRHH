namespace Modulo.Empleados.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin,empleado,evaluador,reclutador,operador_asistencia,gestor_empleados")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmpleadoService _empleadoService;
        private readonly EmpleadoQueryService _empleadoQueryService;
        private readonly CatalogoService _service;

        public EmployeesController(EmpleadoService empleadoService, EmpleadoQueryService empleadoQueryService, CatalogoService service)
        {
            _empleadoService = empleadoService;
            _empleadoQueryService = empleadoQueryService;
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmpleado([FromBody] CreateEmpleadoRequest request)
        {
            var empleado = await _empleadoService.CreateEmpleadoAsync(request);
            return Ok(empleado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateEmpleado(int id, [FromBody] UpdateEmpleadoRequest request)
        {
            var nuevoEmpleado = await _empleadoService.UpdateEmpleadoAsync(id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEmpleado(int id)
        {
                var result = await _empleadoService.DeleteEmpleadoAsync(id);
                return Ok(new { message = "Empleado eliminado correctamente" });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] int? areaId,[FromQuery] int? puestoId,[FromQuery] int? estatusId,[FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            var result = await _empleadoService.GetFilteredParamsAsync(areaId, puestoId, estatusId, page, pageSize);
            return Ok(result);
        }

        // Listar todos los empleados (SP)
        [HttpGet]
        [Authorize(Roles = "admin,gestor_empleados")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var empleados = await _empleadoQueryService.GetAllAsync(ct);
            return Ok(empleados);
        }

        // Obtener detalle de un empleado por ID (SP)
        [HttpGet("{id:int}/detalle")]
        [Authorize(Roles = "admin,gestor_empleados")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
        {
            var empleado = await _empleadoQueryService.GetByIdAsync(id, ct);
            return Ok(empleado);
        }

        // Obtener el perfil del empleado autenticado (SP)
        [HttpGet("me")]
        [Authorize(Roles = "empleado,admin,gestor_empleados")]
        public async Task<IActionResult> GetMe(CancellationToken ct)
        {
            var empleadoId = GetUsuarioId();
            var me = await _empleadoQueryService.GetMeAsync((int)empleadoId, ct); 
            return me is null ? NotFound() : Ok(me);
        }

        // Actualizar el perfil del empleado autenticado (SP)
        [HttpPut("me")]
        [Authorize(Roles = "empleado,admin,gestor_empleados")]
        public async Task<IActionResult> UpdateMe([FromBody] MeProfileUpdateRequest request, CancellationToken ct)
        {
            var empleadoId = GetUsuarioId();
            var updated = await _empleadoQueryService.UpdateMeAsync((int)empleadoId, request, ct);
            return updated is null ? NotFound() : Ok(updated);
        }

        // Extraer el ID del usuario autenticado desde el JWT
        private long GetUsuarioId()
        {
            var val = User.FindFirstValue("empleado_id");
            if (string.IsNullOrWhiteSpace(val))
                throw new InvalidOperationException("El JWT no contiene el empleado_id.");
            return int.Parse(val);
        }

        [HttpGet("areas")]
        public async Task<IActionResult> GetAreas()
        {
            var data = await _service.ObtenerAreas();
            return Ok(data);
        }

        [HttpGet("puestos")]
        public async Task<IActionResult> GetPuestos([FromQuery] long? areaId = null)
        {
            var data = await _service.ObtenerPuestos(areaId);
            return Ok(data);
        }

        [HttpGet("turnos")]
        public async Task<IActionResult> GetTurnos()
        {
            var data = await _service.ObtenerTurnos();
            return Ok(data);
        }

        [HttpGet("estatus")]
        public async Task<IActionResult> GetEstatus()
        {
            var data = await _service.ObtenerEstatus();
            return Ok(data);
        }

    }
}
