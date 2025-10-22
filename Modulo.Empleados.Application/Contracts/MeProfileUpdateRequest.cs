namespace Modulo.Empleados.Application.Contracts
{
    public class MeProfileUpdateRequest
    {
        [JsonPropertyName("domicilio")] public MeProfileDto.DomicilioDto Domicilio { get; init; } = default!;
        [JsonPropertyName("contactoEmergencia")] public MeProfileDto.ContactoDto ContactoEmergencia { get; init; } = default!;
    }
}
