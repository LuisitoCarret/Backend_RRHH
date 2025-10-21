using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Empleados.Application.Contracts;

public sealed class EmployeeListItemDto
{
    [JsonPropertyName("id")] public int Id { get; init; }
    [JsonPropertyName("nombre")] public string Nombre { get; init; } = default!;
    [JsonPropertyName("correo")] public string Correo { get; init; } = default!;
    [JsonPropertyName("telefono")] public string Telefono { get; init; } = default!;
    [JsonPropertyName("fechaIngreso")] public string FechaIngreso { get; init; } = default!;
    [JsonPropertyName("area")] public string Area { get; init; } = default!;
    [JsonPropertyName("puesto")] public string Puesto { get; init; } = default!;
    [JsonPropertyName("turno")] public string Turno { get; init; } = default!;
    [JsonPropertyName("estatus")] public string Estatus { get; init; } = default!;
}
