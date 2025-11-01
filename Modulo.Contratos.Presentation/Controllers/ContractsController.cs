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
    public ContractsController(ContratoCommandService svc,EmployeeWithoutContractService employeeService, 
        ContractService contractService,ContractDetailsService contractDetails)
    {
        _svc = svc;
        _employeeService = employeeService;
        _contractService = contractService;
        _contractDetails = contractDetails;
    }

    /// <summary>
    /// Crea un nuevo contrato (estatus siempre se establece como "Vigente").
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateContractRequest req, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(req, ct);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    /// <summary>
    /// Actualiza los datos de un contrato existente.
    /// Nota: el empleado asociado no se puede cambiar y el estatus se fuerza a "Vigente".
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(ContractDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateContractRequest req, CancellationToken ct)
    {
        // Ya no recibimos empleadoId por query; se mantiene internamente.
        var updated = await _svc.UpdateAsync(id, 0, req, ct); // empleadoId no se usa en SP
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>
    /// Elimina lógicamente un contrato existente.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        var ok = await _svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Crea una renovación de contrato y actualiza su fecha de fin.
    /// </summary>
    [HttpPost("{id:int}/renewals")]
    [Authorize(Roles = "admin,gestor_empleados")]
    [ProducesResponseType(typeof(RenewalDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Renew([FromRoute] int id, [FromBody] CreateRenewalRequest req, CancellationToken ct)
    {
        var result = await _svc.RenewAsync(id, req, ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("available-employees")]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetEmpleadosSinContrato()
    {
        var empleados = await _employeeService.ListarEmpleadosSinContratoAsync();
        return Ok(empleados);
    }

    [HttpGet]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetAll(
          [FromQuery] DateTime? fechaInicioDesde = null,
          [FromQuery] DateTime? fechaFinHasta = null,
          [FromQuery] int? tipoContratoId = null,
          [FromQuery] int? estatusContratoId = null)
    {
        var contratos = await _contractService.ListarContratosAsync(fechaInicioDesde, fechaFinHasta, tipoContratoId, estatusContratoId);
        return Ok(contratos);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetContratoDetalle(int id)
    {
        var contrato = await _contractDetails.ObtenerDetalleAsync(id);

        if (contrato == null)
            return NotFound(new { mensaje = "Contrato no encontrado" });

        return Ok(contrato);
    }

    [HttpGet("types")]
    [Authorize(Roles = "admin,gestor_empleados")]
    public async Task<IActionResult> GetTiposContrato()
    {
        var tipos = await _contractService.ListarTiposContratoAsync();
        return Ok(tipos);
    }


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
