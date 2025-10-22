namespace Modulo.Empleados.Domain.Interfaces
{
    public interface IEmpleadoStoredRepository
    {
        Task<List<Empleado>> GetAllAsync(CancellationToken ct);
        Task<Empleado?> GetByIdAsync(int id, CancellationToken ct);
        Task<(DomicilioEmpleado?, ContactoEmergencia?)?> GetMeAsync(int empleadoId, CancellationToken ct);
        Task<(DomicilioEmpleado?, ContactoEmergencia?)?> UpdateMeAsync(
            int empleadoId,
            DomicilioEmpleado domicilio,
            ContactoEmergencia contacto,
            CancellationToken ct);
    }
}
