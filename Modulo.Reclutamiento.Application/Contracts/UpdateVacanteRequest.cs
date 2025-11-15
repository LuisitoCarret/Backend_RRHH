namespace Modulo.Reclutamiento.Application.Contracts
{
    public class UpdateVacanteRequest
    {
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public int? AreaId { get; set; }
        public int? PuestoId { get; set; }
    }
}
