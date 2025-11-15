// Modulo.Evaluaciones.Application/Contracts/IndicadoresCatalogoDto.cs
namespace Modulo.Evaluaciones.Application.Contracts;
public sealed class IndicadoresCatalogoDto
{
    public int catalogo_id { get; set; }
    public string nombre { get; set; } = default!;
    public string? descripcion { get; set; }
}
