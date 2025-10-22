namespace Modulo.Seguridad.Domain.Interfaces
{
    public interface IUserCreationService
    {
        Task<long> CreateUserWithRoleAsync(string email, string password, string role);
    }
}
