// Modulo.Evaluaciones.Application/Contracts/PlantillasDtos.cs
namespace Modulo.Evaluaciones.Application.Contracts;

public sealed class PlantillaListItemDto
{
    public int plantilla_id { get; set; }
    public string nombre { get; set; } = default!;
    public int area_id { get; set; }
    public string nombre_area { get; set; } = default!;
    public DateTime periodo_inicio { get; set; }
    public DateTime periodo_fin { get; set; }
    public bool vigente { get; set; }
}

public sealed class PlantillaIndicadorDto
{
    public int indicador_id { get; set; }
    public int catalogo_id { get; set; }
    public string nombre { get; set; } = default!;
    public decimal ponderacion { get; set; }
}

public sealed class PlantillaDetalleDto
{
    public int plantilla_id { get; set; }
    public string nombre { get; set; } = default!;
    public string? descripcion { get; set; }
    public int area_id { get; set; }
    public string nombre_area { get; set; } = default!;
    public DateTime periodo_inicio { get; set; }
    public DateTime periodo_fin { get; set; }
    public bool vigente { get; set; }
    public List<PlantillaIndicadorDto> indicadores { get; set; } = new();
}
