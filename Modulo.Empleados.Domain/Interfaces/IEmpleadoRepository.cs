using Modulo.Empleados.Domain.Filter;

namespace Modulo.Empleados.Domain.Interfaces
{
    public interface IEmpleadoRepository
    {
        Task<int> CreateAsync(Empleado empleado);
        Task CreateDomicilioAsync(DomicilioEmpleado domicilio);
        Task CreateContactoEmergenciaAsync(ContactoEmergencia contacto);
        Task<bool> UpdateAsync(Empleado empleado);
        Task<bool> DeleteAsync(int id);
        Task DeleteDomicilioAsync(int domicilioId);
        Task DeleteContactoEmergenciaAsync(int contactoId);
        Task<Empleado> GetByIdAsync(int id);
        Task<PagedResult<Empleado>> GetFilteredAsync(int? areaId, int? puestoId, int? estatusId, int page, int pageSize);

    }
}
