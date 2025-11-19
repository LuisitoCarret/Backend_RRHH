using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Modulo.Empleados.Application.Contracts;
using Modulo.Empleados.Application.Dto;
using Modulo.Empleados.Application.Services;
using Modulo.Empleados.Domain.Entities;
using Modulo.Empleados.Domain.Filter;
using System.Security.Claims;

namespace Modulo.Empleados.Presentation.Controllers;

/// <summary>
/// Controlador para la gestión de empleados dentro del sistema.
/// </summary>
/// <remarks>
/// Este módulo permite:
/// 
/// ✔ Registrar empleados nuevos  
/// ✔ Actualizar información general o del perfil del empleado  
/// ✔ Eliminar empleados  
/// ✔ Consultar empleados con filtros o paginación  
/// ✔ Obtener catálogos (áreas, puestos, turnos, estatus)  
/// 
/// Incluye endpoints especiales para que el empleado autenticado consulte
/// y actualice su propio perfil ("me").
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize(Roles = "admin,empleado,evaluador,reclutador,operador_asistencia,gestor_empleados")]
public class EmployeesController : ControllerBase
{
    private readonly EmpleadoService _empleadoService;
    private readonly EmpleadoQueryService _empleadoQueryService;
    private readonly CatalogoService _catalogoService;

    public EmployeesController(
        EmpleadoService empleadoService,
        EmpleadoQueryService empleadoQueryService,
        CatalogoService catalogoService)
    {
        _empleadoService = empleadoService;
        _empleadoQueryService = empleadoQueryService;
        _catalogoService = catalogoService;
    }

    // ============================================================
    // POST: Crear empleado
    // ============================================================

    /// <summary>
    /// Crea un nuevo empleado junto con su usuario, domicilio y contacto de emergencia.
    /// </summary>
    /// <remarks>
    /// Ejemplo:
    /// 
    ///     POST /api/employees
    ///     {
    ///         "nombre": "Carlos López",
    ///         "email": "carlos@empresa.com",
    ///         "telefono": "5544332211",
    ///         "fechaIngreso": "2025-01-01",
    ///         "areaId": 2,
    ///         "puestoId": 5,
    ///         "turnoId": 1,
    ///         "estatusId": 1,
    ///         "password": "12345678",
    ///         "rol": "empleado",
    ///         "domicilio": { ... },
    ///         "contacto": { ... }
    ///     }
    /// </remarks>
    /// <response code="200">Empleado creado correctamente.</response>
    /// <response code="400">Datos inválidos o error de negocio.</response>
    /// <response code="401">Usuario no autenticado.</response>
    /// <response code="403">Usuario sin permisos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(EmpleadoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateEmpleado([FromBody] CreateEmpleadoRequest request)
    {
        try
        {
            var empleado = await _empleadoService.CreateEmpleadoAsync(request);
            return Ok(empleado);
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // ============================================================
    // PUT: Actualizar empleado
    // ============================================================

    /// <summary>
    /// Actualiza los datos generales de un empleado.
    /// </summary>
    /// <remarks>
    /// Este endpoint no devuelve cuerpo, únicamente confirma la operación.
    /// </remarks>
    /// <response code="200">Empleado actualizado.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="404">Empleado no encontrado.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateEmpleado(int id, [FromBody] UpdateEmpleadoRequest request)
    {
        await _empleadoService.UpdateEmpleadoAsync(id, request);
        return Ok();
    }

    // ============================================================
    // DELETE: Eliminar empleado
    // ============================================================

    /// <summary>
    /// Elimina un empleado por su ID.
    /// </summary>
    /// <response code="200">Empleado eliminado correctamente.</response>
    /// <response code="404">Empleado no encontrado.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteEmpleado(int id)
    {
        await _empleadoService.DeleteEmpleadoAsync(id);
        return Ok(new { message = "Empleado eliminado correctamente" });
    }

    // ============================================================
    // GET: Filtro paginado
    // ============================================================

    /// <summary>
    /// Obtiene empleados filtrados por área, puesto o estatus, con paginación.
    /// </summary>
    /// <response code="200">Lista filtrada.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpGet("filter")]
    [ProducesResponseType(typeof(PagedResult<EmployeeListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetFiltered(
        [FromQuery] int? areaId,
        [FromQuery] int? puestoId,
        [FromQuery] int? estatusId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _empleadoService.GetFilteredParamsAsync(areaId, puestoId, estatusId, page, pageSize);
        return Ok(result);
    }

    // ============================================================
    // GET: Listar todos los empleados
    // ============================================================

    /// <summary>
    /// Obtiene todos los empleados registrados (sin paginación).
    /// </summary>
    /// <response code="200">Lista de empleados.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpGet]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(List<EmployeeListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var empleados = await _empleadoQueryService.GetAllAsync(ct);
        return Ok(empleados);
    }

    // ============================================================
    // GET: Detalle por ID
    // ============================================================

    /// <summary>
    /// Obtiene información detallada de un empleado por su ID.
    /// </summary>
    /// <response code="200">Detalle encontrado.</response>
    /// <response code="404">Empleado no encontrado.</response>
    /// <response code="401">No autenticado.</response>
    /// <response code="403">Sin permisos.</response>
    [HttpGet("{id:int}/detalle")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(EmployeeDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
    {
        var empleado = await _empleadoQueryService.GetByIdAsync(id, ct);
        return empleado is null ? NotFound() : Ok(empleado);
    }

    // ============================================================
    // GET: Perfil del empleado autenticado
    // ============================================================

    /// <summary>
    /// Obtiene el perfil del empleado autenticado.
    /// </summary>
    /// <response code="200">Perfil encontrado.</response>
    /// <response code="404">Empleado no encontrado.</response>
    /// <response code="401">No autenticado.</response>
    [HttpGet("me")]
    [Authorize(Roles = "empleado,admin,gestor_empleados")]
    [ProducesResponseType(typeof(MeProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var empleadoId = GetUsuarioId();
        var me = await _empleadoQueryService.GetMeAsync((int)empleadoId, ct);
        return me is null ? NotFound() : Ok(me);
    }

    // ============================================================
    // PUT: Actualizar perfil del empleado autenticado
    // ============================================================

    /// <summary>
    /// Actualiza la información del perfil del empleado autenticado.
    /// </summary>
    /// <response code="200">Perfil actualizado.</response>
    /// <response code="404">Empleado no encontrado.</response>
    /// <response code="401">No autenticado.</response>
    [HttpPut("me")]
    [Authorize(Roles = "empleado,admin,gestor_empleados")]
    [ProducesResponseType(typeof(MeProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateMe([FromBody] MeProfileUpdateRequest request, CancellationToken ct)
    {
        var empleadoId = GetUsuarioId();
        var updated = await _empleadoQueryService.UpdateMeAsync((int)empleadoId, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    // ============================================================
    // GET: Catálogos
    // ============================================================

    /// <summary>
    /// Obtiene la lista de áreas registradas.
    /// </summary>
    [HttpGet("areas")]
    [ProducesResponseType(typeof(List<Area>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAreas()
    {
        var data = await _catalogoService.ObtenerAreas();
        return Ok(data);
    }

    /// <summary>
    /// Obtiene la lista de puestos.  
    /// Puede filtrar por área.
    /// </summary>
    [HttpGet("puestos")]
    [ProducesResponseType(typeof(List<Puesto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPuestos([FromQuery] long? areaId = null)
    {
        var data = await _catalogoService.ObtenerPuestos(areaId);
        return Ok(data);
    }

    /// <summary>
    /// Obtiene la lista de turnos.
    /// </summary>
    [HttpGet("turnos")]
    [ProducesResponseType(typeof(List<Turno>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTurnos()
    {
        var data = await _catalogoService.ObtenerTurnos();
        return Ok(data);
    }

    /// <summary>
    /// Obtiene la lista de estatus de empleados.
    /// </summary>
    [HttpGet("estatus")]
    [ProducesResponseType(typeof(List<EstatusEmpleado>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEstatus()
    {
        var data = await _catalogoService.ObtenerEstatus();
        return Ok(data);
    }

    // ============================================================
    // Helper
    // ============================================================

    private long GetUsuarioId()
    {
        var val = User.FindFirstValue("empleado_id");
        if (string.IsNullOrWhiteSpace(val))
            throw new InvalidOperationException("El JWT no contiene el empleado_id.");
        return int.Parse(val);
    }
}
