namespace Modulo.Contratos.Application.Contracts
{
    public class ContractListDto
    {
        public int ContratoId { get; set; }     
        public int EmpleadoId { get; set; }
        public string NombreEmpleado { get; set; }
        public string TipoContrato { get; set; }
        public string EstatusContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
