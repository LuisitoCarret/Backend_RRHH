namespace Modulo.Contratos.Application.Interfaces
{
    public interface IEmployeeWithoutContract
    {
        Task<IEnumerable<EmployeeWithoutContractDto>> ObtenerEmpleadosSinContratoAsync();
    }
}
