using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ==========================
// Modulo.Reclutamiento.Application/Contracts/VacantesContracts.cs
// ==========================
namespace Modulo.Reclutamiento.Application.Contracts;

public sealed class CreateVacanteRequest
{
    public string Titulo { get; set; } = default!;
    public string? Descripcion { get; set; }
    public int AreaId { get; set; }
    public int PuestoId { get; set; }
    public DateTime FechaPublicacion { get; set; }
}

public sealed class VacanteDetailDto
{
    public int VacanteId { get; set; }
    public string Titulo { get; set; } = default!;
    public string? Descripcion { get; set; }
    public int AreaId { get; set; }
    public string NombreArea { get; set; } = default!;
    public int PuestoId { get; set; }
    public string NombrePuesto { get; set; } = default!;
    public string Estatus { get; set; } = default!;
    public DateTime FechaPublicacion { get; set; }
    public DateTime? FechaCierre { get; set; }
}

public sealed class VacanteItemDto
{
    public int VacanteId { get; set; }
    public string Titulo { get; set; } = default!;
    public int AreaId { get; set; }
    public string NombreArea { get; set; } = default!;
    public int PuestoId { get; set; }
    public string NombrePuesto { get; set; } = default!;
    public string Estatus { get; set; } = default!;
    public DateTime FechaPublicacion { get; set; }
}

public sealed class ListVacantesRequest
{
    public string? Estatus { get; set; }
    public int? AreaId { get; set; }
    public int? PuestoId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public sealed class ListVacantesResponseDto
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<VacanteItemDto> Items { get; set; } = new();
}
