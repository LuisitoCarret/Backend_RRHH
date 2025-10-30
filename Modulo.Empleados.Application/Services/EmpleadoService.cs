namespace Modulo.Empleados.Application.Services
{
    public class EmpleadoService
    {
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IUsuarioEmpleadoRepository _usuarioEmpleadoRepository;
        private readonly IUserCreationService _userCreationService;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public EmpleadoService(IEmpleadoRepository empleadoRepository, IUsuarioEmpleadoRepository usuarioEmpleadoRepository,
        IUserCreationService userCreationService, IUsuarioRepository usuarioRepository,IMapper mapper)
        {
            _empleadoRepository = empleadoRepository;
            _usuarioEmpleadoRepository = usuarioEmpleadoRepository;
            _userCreationService = userCreationService;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateEmpleadoAsync(CreateEmpleadoRequest request)
        {
            // Crear empleado
            var empleado = _mapper.Map<Empleado>(request);
            var empleadoId = await _empleadoRepository.CreateAsync(empleado);

            // Crear usuario y rol en Seguridad
            var usuarioId = await _userCreationService.CreateUserWithRoleAsync(
                request.Email,
                request.Password,
                request.Rol
            );

            // Crear relación usuario ↔ empleado
            var relacion = new UsuarioEmpleado
            {
                UsuarioId = usuarioId,
                EmpleadoId = empleadoId
            };

            await _usuarioEmpleadoRepository.CreateAsync(relacion);

            // Crear domicilio
            var domicilio = _mapper.Map<DomicilioEmpleado>(request.Domicilio);
            domicilio.EmpleadoId = empleadoId;
            await _empleadoRepository.CreateDomicilioAsync(domicilio);

            // Crear contacto de emergencia
            var contacto = _mapper.Map<ContactoEmergencia>(request.Contacto);
            contacto.EmpleadoId = empleadoId;
            await _empleadoRepository.CreateContactoEmergenciaAsync(contacto);

            return empleadoId;
        }   

        public async Task<bool> UpdateEmpleadoAsync(int id,UpdateEmpleadoRequest request)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null) return false;

            // actualizar propiedades
            empleado.Nombre = request.Nombre ?? empleado.Nombre;
            empleado.Email = request.Email ?? empleado.Email;
            empleado.Telefono = request.Telefono ?? empleado.Telefono;
            empleado.AreaId = request.AreaId != 0 ? request.AreaId : empleado.AreaId;
            empleado.PuestoId = request.PuestoId != 0 ? request.PuestoId : empleado.PuestoId;
            empleado.TurnoId = request.TurnoId != 0 ? request.TurnoId : empleado.TurnoId;
            empleado.EstatusId = request.EstatusId != 0 ? (short)request.EstatusId : empleado.EstatusId;

            return await _empleadoRepository.UpdateAsync(empleado);
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            var empleado = await _empleadoRepository.GetByIdAsync(id);
            if (empleado == null)
                return false;

            try
            {
                //Buscar relación usuario ↔ empleado
                var relacion = await _usuarioEmpleadoRepository.GetByEmpleadoIdAsync(id);

                if (relacion != null)
                {
                    long usuarioId = relacion.UsuarioId;

                    // Eliminar roles del usuario
                    await _usuarioRepository.DeleteRolesByUsuarioIdAsync(usuarioId);

                    // Eliminar relación usuario_empleado
                    await _usuarioEmpleadoRepository.DeleteAsync(usuarioId, relacion.EmpleadoId);

                    // Eliminar usuario
                    await _usuarioRepository.DeleteAsync(usuarioId);
                }

                // Eliminar domicilio si existe
                if (empleado.Domicilio != null)
                    await _empleadoRepository.DeleteDomicilioAsync(empleado.Domicilio.Id);

                // Eliminar contacto si existe
                if (empleado.ContactoEmergencia != null)
                    await _empleadoRepository.DeleteContactoEmergenciaAsync(empleado.ContactoEmergencia.Id);

                // Eliminar empleado
                return await _empleadoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<PagedResult<EmpleadoResponse>> GetFilteredParamsAsync(int? areaId, int? puestoId, int? estatusId, int page, int pageSize)
        {
            var result = await _empleadoRepository.GetFilteredAsync(areaId, puestoId, estatusId, page, pageSize);
            return new PagedResult<EmpleadoResponse>
            {
                Items = _mapper.Map<IEnumerable<EmpleadoResponse>>(result.Items),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            };
        }
    }
}
