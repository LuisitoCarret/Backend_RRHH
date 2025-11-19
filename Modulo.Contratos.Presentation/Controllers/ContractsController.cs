

namespace Modulo.Contratos.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class ContractsController : ControllerBase
{
    private readonly ContratoCommandService _svc;
    private readonly EmployeeWithoutContractService _employeeService;
    private readonly ContractService _contractService;
    private readonly ContractDetailsService _contractDetails;

    public ContractsController(
        ContratoCommandService svc,
        EmployeeWithoutContractService employeeService,
        ContractService contractService,
        ContractDetailsService contractDetails)
    {
        _svc = svc;
        _employeeService = employeeService;
        _contractService = contractService;
        _contractDetails = contractDetails;
    }

    // ========================================================================
    // POST: Crear contrato
    // ========================================================================

    /// <summary>
    /// Crea un nuevo contrato para un empleado.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite registrar un nuevo contrato para un empleado.
    /// El estatus siempre se fuerza internamente a <b>"Vigente"</b>.
    ///
    /// <h4>Ejemplo de solicitud</h4>
    /// <code>
    /// {
    ///   "empleadoId": 12,
    ///   "tipoContratoId": 1,
    ///   "estatusContratoId": 1,
    ///   "fechaInicio": "2025-01-01",
    ///   "fechaFin": "2025-06-30",
    ///   "salarioBase": 15000,
    ///   "observaciones": "Contrato inicial"
    /// }
    /// </code>
    ///
    /// <h4>Ejemplo de respuesta (201)</h4>
    /// <code>
    /// {
    ///   "id": 85,
    ///   "empleadoId": 12,
    ///   "tipoContratoId": 1,
    ///   "estatusContratoId": 1,
    ///   "fechaInicio": "2025-01-01",
    ///   "fechaFin": "2025-06-30",
    ///   "salarioBase": 15000,
    ///   "observaciones": "Contrato inicial"
    /// }
    /// </code>
    /// </remarks>
    /// <response code="201">Contrato creado exitosamente.</response>
    /// <response code="400">Error de validación o regla de negocio violada.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPost]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest req, CancellationToken ct)
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
                Title = "Regla de negocio violada",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Error SQL",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Error interno del servidor",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    // ========================================================================
    // PUT: Actualizar contrato
    // ========================================================================

    /// <summary>
    /// Actualiza los datos de un contrato existente.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite actualizar campos de un contrato como fechas, salario u observaciones.
    /// El empleado asociado no se puede cambiar.
    ///
    /// <h4>Ejemplo de solicitud</h4>
    /// <code>
    /// {
    ///   "tipoContratoId": 1,
    ///   "estatusContratoId": 1,
    ///   "fechaInicio": "2025-01-01",
    ///   "fechaFin": "2025-12-31",
    ///   "salarioBase": 18000,
    ///   "observaciones": "Actualización de salario"
    /// }
    /// </code>
    /// </remarks>
    /// <response code="200">Contrato actualizado exitosamente.</response>
    /// <response code="404">Contrato no encontrado.</response>
    /// <response code="400">Error de validación o regla de negocio violada.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateContractRequest req, CancellationToken ct)
    {
        try
        {
            var updated = await _svc.UpdateAsync(id, 0, req, ct);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Regla de negocio violada",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Error SQL",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Error interno del servidor",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    // ========================================================================
    // DELETE: Eliminar contrato
    // ========================================================================

    /// <summary>
    /// Elimina lógicamente un contrato existente.
    /// </summary>
    /// <remarks>
    /// Marca el contrato como eliminado (borrado lógico) sin removerlo físicamente.
    /// </remarks>
    /// <response code="204">Contrato eliminado exitosamente.</response>
    /// <response code="404">Contrato no encontrado.</response>
    /// <response code="400">Error de negocio o SQL.</response>
    /// <response code="500">Error interno del servidor.</response>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        try
        {
            var ok = await _svc.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Regla de negocio violada",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Error SQL",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Error interno del servidor",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    // ========================================================================
    // POST: Renovación de contrato
    // ========================================================================

    /// <summary>
    /// Crea una renovación de contrato para extender su fecha de fin.
    /// </summary>
    /// <remarks>
    /// Este endpoint registra un historial de renovación y actualiza la fecha_fin del contrato existente.
    /// </remarks>
    [HttpPost("{id:int}/renewals")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(RenewalDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Renew([FromRoute] int id, [FromBody] CreateRenewalRequest req, CancellationToken ct)
    {
        try
        {
            var result = await _svc.RenewAsync(id, req, ct);
            return StatusCode(StatusCodes.Status201Created, result);
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Regla de negocio violada",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (SqlException ex)
        {
            return BadRequest(new ValidationProblemDetails
            {
                Title = "Error SQL",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "Error interno del servidor",
                Detail = ex.Message,
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    // ========================================================================
    // GET: Empleados disponibles para contratos
    // ========================================================================

    /// <summary>
    /// Lista empleados que no tienen un contrato vigente.
    /// </summary>
    [HttpGet("available-employees")]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetEmpleadosSinContrato()
    {
        var empleados = await _employeeService.ListarEmpleadosSinContratoAsync();
        return Ok(empleados);
    }

    // ========================================================================
    // GET: Listado general de contratos
    // ========================================================================

    /// <summary>
    /// Lista contratos aplicando filtros opcionales.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? fechaInicioDesde = null,
        [FromQuery] DateTime? fechaFinHasta = null,
        [FromQuery] int? tipoContratoId = null,
        [FromQuery] int? estatusContratoId = null)
    {
        var contratos = await _contractService.ListarContratosAsync(
            fechaInicioDesde, fechaFinHasta, tipoContratoId, estatusContratoId);

        return Ok(contratos);
    }

    // ========================================================================
    // GET: Detalle de contrato
    // ========================================================================

    /// <summary>
    /// Devuelve la información detallada de un contrato.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetContratoDetalle(int id)
    {
        var contrato = await _contractDetails.ObtenerDetalleAsync(id);
        if (contrato == null)
            return NotFound(new { mensaje = "Contrato no encontrado" });

        return Ok(contrato);
    }

    // ========================================================================
    // GET: Tipos de contrato
    // ========================================================================

    /// <summary>
    /// Devuelve los tipos de contrato disponibles.
    /// </summary>
    [HttpGet("types")]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetTiposContrato()
    {
        var tipos = await _contractService.ListarTiposContratoAsync();
        return Ok(tipos);
    }

    // ========================================================================
    // GET: Contrato vigente del empleado autenticado
    // ========================================================================

    /// <summary>
    /// Devuelve el contrato vigente del usuario autenticado (rol empleado).
    /// </summary>
    [HttpGet("me")]
    [Authorize(Roles = "empleado")]
    public async Task<IActionResult> GetMyContract()
    {
        var contrato = await _contractService.ObtenerContratoVigenteActualAsync(User);
        if (contrato == null)
            return NotFound(new { mensaje = "No se encontró un contrato vigente." });

        return Ok(contrato);
    }
}
