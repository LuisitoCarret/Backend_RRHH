namespace Modulo.Asistencias.Application.Services;

public sealed class AsistenciasService
{
    private readonly IAsistenciasRepository _repo;
    private readonly IHttpContextAccessor _httpContext;
    public AsistenciasService(IAsistenciasRepository repo, IHttpContextAccessor httpContext)
    {
        _repo = repo;
        _httpContext = httpContext;
    }

    public async Task<IReadOnlyList<AsistenciaListItemDto>> ListarAsync(DateTime? desde, DateTime? hasta, int? turnoId, CancellationToken ct)
    {
        if (desde is null && hasta is null)
        {
            var hoy = DateTime.Today;
            desde = hoy;
            hasta = hoy;
        }

        var registros = await _repo.ListarAsync(desde, hasta, turnoId, ct);
        return registros.Select(r => r.ToDto()).ToList();
    }
        

    public async Task<AsistenciaInsertResponseDto> InsertarAsync(AsistenciaInsertRequest req, CancellationToken ct)
    {
        var empleadoIdClaim = _httpContext.HttpContext?.User.FindFirst("empleado_id")
                              ?? _httpContext.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (empleadoIdClaim is null)
            throw new BusinessRuleException("Token sin identificador de empleado.");

        int empleadoId = int.Parse(empleadoIdClaim.Value);

        var entity = await _repo.InsertarAsync(empleadoId, req.TipoRegistro, ct);
        return entity.ToDto();
    }

    public async Task<AsistenciaInsertResponseDto> ActualizarAsync(int asistenciaId, AsistenciaUpdateRequest req, CancellationToken ct)
    {
        var entrada = string.IsNullOrWhiteSpace(req.HoraEntrada) ? (TimeSpan?)null : TimeSpan.Parse(req.HoraEntrada);
        var salida = string.IsNullOrWhiteSpace(req.HoraSalida) ? (TimeSpan?)null : TimeSpan.Parse(req.HoraSalida);

        var result = await _repo.ActualizarAsync(asistenciaId, entrada, salida, req.Observaciones, ct);
        return result.ToDto();
    }

    public async Task<IReadOnlyList<AsistenciaReporteMensualDto>> ReporteMensualAsync(DateTime fechaInicio, DateTime fechaFin, CancellationToken ct)
        => (await _repo.ReporteMensualAsync(fechaInicio, fechaFin, ct)).Select(r => r.ToDto()).ToList();


}
