// Modulo.Evaluaciones.Application/Contracts/UpdateVigenciaRequest.cs
namespace Modulo.Evaluaciones.Application.Contracts;
public sealed class UpdateVigenciaRequest { public bool vigente { get; set; } }
public sealed class UpdateVigenciaResponse { public string mensaje { get; set; } = default!; }
