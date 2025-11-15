// Modulo.Evaluaciones.Domain/Entities/PlantillaDetalle.cs
namespace Modulo.Evaluaciones.Domain.Entities;
public sealed class PlantillaDetalle
{
    public int PlantillaId { get; set; }
    public string Nombre { get; set; } = default!;
    public string? Descripcion { get; set; }
    public int AreaId { get; set; }
    public string NombreArea { get; set; } = default!;
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFin { get; set; }
    public bool Vigente { get; set; }
    public List<PlantillaIndicadorDetalle> Indicadores { get; set; } = new();
}

public sealed class PlantillaIndicadorDetalle
{
    public int IndicadorId { get; set; }
    public int CatalogoId { get; set; }
    public string NombreIndicador { get; set; } = default!;
    public decimal Ponderacion { get; set; }
}
