// Modulo.Evaluaciones.Presentation/Controllers/IndicadoresCatalogoController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Modulo.Evaluaciones.Application.Services;

namespace Modulo.Evaluaciones.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class IndicadoresCatalogoController : ControllerBase
{
    private readonly IndicadoresCatalogoService _svc;
    public IndicadoresCatalogoController(IndicadoresCatalogoService svc) => _svc = svc;

    [HttpGet]
    [Authorize(Roles = "admin,evaluador")]
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
