// Modulo.Evaluaciones.Application/Contracts/CreatePlantillaRequest.cs
namespace Modulo.Evaluaciones.Application.Contracts;

public sealed class CreatePlantillaRequest
{
    public string nombre { get; set; } = default!;
    public string? descripcion { get; set; }
    public int area_id { get; set; }
    public DateTime periodo_inicio { get; set; }
    public DateTime periodo_fin { get; set; }
    public List<CreatePlantillaIndicadorItem> indicadores { get; set; } = new();
}

public sealed class CreatePlantillaIndicadorItem
{
    public int catalogo_id { get; set; }
    public decimal ponderacion { get; set; }
}

public sealed class CreatePlantillaResponse
{
    public int plantilla_id { get; set; }
    public string mensaje { get; set; } = default!;
}
