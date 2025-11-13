namespace Modulo.Contratos.Application.Contracts
{
    public class ContractDetailDto
    {
        public int ContratoId { get; set; }
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; }
        public string TipoContrato { get; set; }
        public string EstatusContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal SalarioBase { get; set; }
        public string Observaciones { get; set; }

        public List<RenewalDto> Renovaciones { get; set; } = new();
    }
}
