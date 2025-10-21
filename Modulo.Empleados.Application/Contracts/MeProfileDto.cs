using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Empleados.Application.Contracts;

public sealed class MeProfileDto
{
    [JsonPropertyName("domicilio")] public DomicilioDto? Domicilio { get; init; }
    [JsonPropertyName("contactoEmergencia")] public ContactoDto? ContactoEmergencia { get; init; }

    public sealed class DomicilioDto
    {
        [JsonPropertyName("calle")] public string Calle { get; init; } = default!;
        [JsonPropertyName("numero")] public string Numero { get; init; } = default!;
        [JsonPropertyName("colonia")] public string Colonia { get; init; } = default!;
        [JsonPropertyName("ciudad")] public string Ciudad { get; init; } = default!;
        [JsonPropertyName("estado")] public string Estado { get; init; } = default!;
        [JsonPropertyName("codigoPostal")] public string CodigoPostal { get; init; } = default!;
    }

    public sealed class ContactoDto
    {
        [JsonPropertyName("nombre")] public string Nombre { get; init; } = default!;
        [JsonPropertyName("parentesco")] public string Parentesco { get; init; } = default!;
        [JsonPropertyName("telefono")] public string Telefono { get; init; } = default!;
    }
}
