namespace Modulo.Seguridad.Application.Services
{
    public enum LoginFailureReason
    {
        None,
        InvalidInput,
        InvalidEmail,
        InvalidPassword,
        Inactive,
        Locked
    }
}
