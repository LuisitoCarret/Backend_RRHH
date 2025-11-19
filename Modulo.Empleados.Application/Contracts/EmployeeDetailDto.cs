namespace Modulo.Empleados.Application.Contracts
{
    public class EmployeeDetailDto
    {
        [JsonPropertyName("id")] public int Id { get; init; }
        [JsonPropertyName("nombre")] public string Nombre { get; init; } = default!;
        [JsonPropertyName("correo")] public string Correo { get; init; } = default!;
        [JsonPropertyName("telefono")] public string Telefono { get; init; } = default!;
        [JsonPropertyName("fechaIngreso")] public string FechaIngreso { get; init; } = default!;
        [JsonPropertyName("area_id")] public int AreaId { get; init; } = default!;
        [JsonPropertyName("area")] public string Area { get; init; } = default!;
        [JsonPropertyName("puesto_id")] public int PuestoId { get; init; } = default!;
        [JsonPropertyName("puesto")] public string Puesto { get; init; } = default!;
        [JsonPropertyName("turno_id")] public int TurnoId { get; init; } = default!;
        [JsonPropertyName("turno")] public string Turno { get; init; } = default!;
        [JsonPropertyName("estatus_id")] public int EstatusId { get; init; } = default!;
        [JsonPropertyName("estatus")] public string Estatus { get; init; } = default!;
        [JsonPropertyName("domicilio")] public MeProfileDto.DomicilioDto? Domicilio { get; init; }
        [JsonPropertyName("contactoEmergencia")] public MeProfileDto.ContactoDto? ContactoEmergencia { get; init; }
    }
}
