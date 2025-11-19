namespace Modulo.Reclutamiento.Application.Contracts
{
    public class UpdatePostulacionRequest
    {
        public string Estatus { get; set; } = default!;
        public string? Observacion { get; set; }
    }
}
