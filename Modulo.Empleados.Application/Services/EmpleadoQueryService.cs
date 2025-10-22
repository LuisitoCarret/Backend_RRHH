using Modulo.Empleados.Domain.Entities;

namespace Modulo.Empleados.Application.Services
{
    public class EmpleadoQueryService
    {
        private readonly IEmpleadoStoredRepository _repo;

        public EmpleadoQueryService(IEmpleadoStoredRepository repo)
        {
            _repo = repo;
        }

         public async Task<IEnumerable<EmployeeListItemDto>> GetAllAsync(CancellationToken ct)
        {
            var empleados = await _repo.GetAllAsync(ct);
            return empleados.Select(e => new EmployeeListItemDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Correo = e.Email,
                Telefono = e.Telefono,
                FechaIngreso = e.FechaIngreso.ToString("yyyy-MM-dd"),
                Area = e.Area?.Nombre ?? "",
                Puesto = e.Puesto?.Nombre ?? "",
                Turno = e.Turno?.Nombre ?? "",
                Estatus = e.Estatus?.Nombre ?? ""
            });
        }

        public async Task<EmployeeDetailDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            var e = await _repo.GetByIdAsync(id, ct);
            if (e == null) return null;

            return new EmployeeDetailDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Correo = e.Email,
                Telefono = e.Telefono,
                FechaIngreso = e.FechaIngreso.ToString("yyyy-MM-dd"),
                Area = e.Area?.Nombre ?? "",
                Puesto = e.Puesto?.Nombre ?? "",
                Turno = e.Turno?.Nombre ?? "",
                Estatus = e.Estatus?.Nombre ?? "",
                Domicilio = e.Domicilio == null ? null : new MeProfileDto.DomicilioDto
                {
                    Calle = e.Domicilio.Calle,
                    Numero = e.Domicilio.Numero,
                    Colonia = e.Domicilio.Colonia,
                    Ciudad = e.Domicilio.Ciudad,
                    Estado = e.Domicilio.Estado,
                    CodigoPostal = e.Domicilio.CodigoPostal
                },
                ContactoEmergencia = e.ContactoEmergencia == null ? null : new MeProfileDto.ContactoDto
                {
                    Nombre = e.ContactoEmergencia.Nombre,
                    Parentesco = e.ContactoEmergencia.Parentesco,
                    Telefono = e.ContactoEmergencia.Telefono
                }
            };
        }

        public async Task<MeProfileDto?> GetMeAsync(int empleadoId, CancellationToken ct)
        {
            var me = await _repo.GetMeAsync(empleadoId, ct);
            if (me == null) return null;

            var (dom, con) = me.Value;
            return new MeProfileDto
            {
                Domicilio = dom == null ? null : new MeProfileDto.DomicilioDto
                {
                    Calle = dom.Calle,
                    Numero = dom.Numero,
                    Colonia = dom.Colonia,
                    Ciudad = dom.Ciudad,
                    Estado = dom.Estado,
                    CodigoPostal = dom.CodigoPostal
                },
                ContactoEmergencia = con == null ? null : new MeProfileDto.ContactoDto
                {
                    Nombre = con.Nombre,
                    Parentesco = con.Parentesco,
                    Telefono = con.Telefono
                }
            };
        }

        public async Task<MeProfileDto?> UpdateMeAsync(int empleadoId, MeProfileUpdateRequest req, CancellationToken ct)
        {
            var domicilio = new DomicilioEmpleado
            {
                Calle = req.Domicilio.Calle,
                Numero = req.Domicilio.Numero,
                Colonia = req.Domicilio.Colonia,
                Ciudad = req.Domicilio.Ciudad,
                Estado = req.Domicilio.Estado,
                CodigoPostal = req.Domicilio.CodigoPostal
            };

            var contacto = new ContactoEmergencia
            {
                Nombre = req.ContactoEmergencia.Nombre,
                Parentesco = req.ContactoEmergencia.Parentesco,
                Telefono = req.ContactoEmergencia.Telefono
            };

            var updated = await _repo.UpdateMeAsync(empleadoId, domicilio, contacto, ct);
            if (updated == null) return null;

            var (dom, con) = updated.Value;
            return new MeProfileDto
            {
                Domicilio = dom == null ? null : new MeProfileDto.DomicilioDto
                {
                    Calle = dom.Calle,
                    Numero = dom.Numero,
                    Colonia = dom.Colonia,
                    Ciudad = dom.Ciudad,
                    Estado = dom.Estado,
                    CodigoPostal = dom.CodigoPostal
                },
                ContactoEmergencia = con == null ? null : new MeProfileDto.ContactoDto
                {
                    Nombre = con.Nombre,
                    Parentesco = con.Parentesco,
                    Telefono = con.Telefono
                }
            };
        }
    }
}
