namespace Modulo.Contratos.Application.Services
{
    public class EmployeeWithoutContractService
    {
        private readonly IEmployeeWithoutContract _empleadoRepository;

        public EmployeeWithoutContractService(IEmployeeWithoutContract empleadoRepository)
        {
            _empleadoRepository = empleadoRepository;
        }

        public async Task<IEnumerable<EmployeeWithoutContractDto>> ListarEmpleadosSinContratoAsync()
        {
            return await _empleadoRepository.ObtenerEmpleadosSinContratoAsync();
        }
    }
}
