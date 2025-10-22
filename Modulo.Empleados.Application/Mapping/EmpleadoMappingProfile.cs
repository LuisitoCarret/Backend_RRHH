namespace Modulo.Empleados.Application.Mapping
{
    public class EmpleadoMappingProfile:Profile
    {
        public EmpleadoMappingProfile()
        {
            CreateMap<CreateEmpleadoRequest, Empleado>()
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
                 .ForMember(dest => dest.AreaId, opt => opt.MapFrom(src => src.AreaId))
                 .ForMember(dest => dest.PuestoId, opt => opt.MapFrom(src => src.PuestoId))
                 .ForMember(dest => dest.TurnoId, opt => opt.MapFrom(src => src.TurnoId))
                 .ForMember(dest => dest.EstatusId, opt => opt.MapFrom(src => src.EstatusId))
                 .ForMember(dest => dest.ContactoEmergencia, opt => opt.Ignore())
                 .ForMember(dest => dest.Domicilio, opt => opt.Ignore());

            CreateMap<CreateEmpleadoRequest.DomicilioRequest, DomicilioEmpleado>();

            CreateMap<CreateEmpleadoRequest.ContactoEmergenciaRequest, ContactoEmergencia>();

            CreateMap<Empleado, EmpleadoResponse>()
               .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area != null ? src.Area.Nombre : string.Empty))
               .ForMember(dest => dest.Puesto, opt => opt.MapFrom(src => src.Puesto != null ? src.Puesto.Nombre : string.Empty))
               .ForMember(dest => dest.Turno, opt => opt.MapFrom(src => src.Turno != null ? src.Turno.Nombre : string.Empty))
               .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus != null ? src.Estatus.Nombre : string.Empty));
       }
    }
}
