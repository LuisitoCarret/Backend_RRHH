// Modulo.Evaluaciones.Domain/Entities/CatalogoIndicador.cs
namespace Modulo.Evaluaciones.Domain.Entities;
public sealed class CatalogoIndicador
{
    public int CatalogoId { get; set; }
    public string Nombre { get; set; } = default!;
    public string? Descripcion { get; set; }
}
