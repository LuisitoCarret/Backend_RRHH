// Modulo.Evaluaciones.Domain/Entities/PlantillaListItem.cs
namespace Modulo.Evaluaciones.Domain.Entities;
public sealed class PlantillaListItem
{
    public int PlantillaId { get; set; }
    public string Nombre { get; set; } = default!;
    public int AreaId { get; set; }
    public string NombreArea { get; set; } = default!;
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFin { get; set; }
    public bool Vigente { get; set; }
}
