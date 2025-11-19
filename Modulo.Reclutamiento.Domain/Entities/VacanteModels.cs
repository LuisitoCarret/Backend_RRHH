namespace Modulo.Reclutamiento.Domain;

public sealed class VacanteCreate
{
    public string Titulo { get; set; } = default!;
    public string? Descripcion { get; set; }
    public int AreaId { get; set; }
    public int PuestoId { get; set; }
    public DateTime FechaPublicacion { get; set; }
}

public sealed class VacanteDetail
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

public sealed class VacanteItem
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

public sealed class VacanteListResponse
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public List<VacanteItem> Items { get; set; } = new();
}