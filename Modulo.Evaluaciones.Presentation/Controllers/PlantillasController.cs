// Modulo.Evaluaciones.Presentation/Controllers/PlantillasController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Modulo.Evaluaciones.Application.Contracts;
using Modulo.Evaluaciones.Application.Services;

namespace Modulo.Evaluaciones.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class PlantillasController : ControllerBase
{
    private readonly PlantillasQueryService _query;
    private readonly PlantillasCommandService _cmd;

    public PlantillasController(PlantillasQueryService query, PlantillasCommandService cmd)
    {
        _query = query; _cmd = cmd;
    }

    // GET /api/Plantillas?area_id=2&vigente=true
    [HttpGet]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(List<PlantillaListItemDto>), StatusCodes.Status200OK)]
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

    // GET /api/Plantillas/{id}
    [HttpGet("{id:int}")]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(PlantillaDetalleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    // POST /api/Plantillas
    [HttpPost]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(CreatePlantillaResponse), StatusCodes.Status201Created)]
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

    // PUT /api/Plantillas/{id}/Vigencia
    [HttpPut("{id:int}/Vigencia")]
    [Authorize(Roles = "admin,evaluador")]
    [ProducesResponseType(typeof(UpdateVigenciaResponse), StatusCodes.Status200OK)]
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
