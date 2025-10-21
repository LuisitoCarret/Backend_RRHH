using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace Modulo.Empleados.Application.Contracts;

public sealed class MeProfileUpdateRequest
{
    [JsonPropertyName("domicilio")] public MeProfileDto.DomicilioDto Domicilio { get; init; } = default!;
    [JsonPropertyName("contactoEmergencia")] public MeProfileDto.ContactoDto ContactoEmergencia { get; init; } = default!;
}
