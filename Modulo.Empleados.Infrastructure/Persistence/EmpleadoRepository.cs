namespace Modulo.Empleados.Infrastructure.Persistence
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly EmpleadosDbContext _context;

        public EmpleadoRepository(EmpleadosDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado.Id;
        }

        public async Task CreateContactoEmergenciaAsync(ContactoEmergencia contacto)
        {
            _context.Contactos.Add(contacto);
            await _context.SaveChangesAsync();  
        }

        public async Task CreateDomicilioAsync(DomicilioEmpleado domicilio)
        {
            _context.Domicilios.Add(domicilio);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados
                   .Include(e => e.Domicilio)
                   .Include(e => e.ContactoEmergencia)
                   .FirstOrDefaultAsync(e => e.Id == id);

            if (empleado == null)
                return false;

            if (empleado.Domicilio != null)
                _context.Domicilios.Remove(empleado.Domicilio);

            if (empleado.ContactoEmergencia != null)
                _context.Contactos.Remove(empleado.ContactoEmergencia);

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteContactoEmergenciaAsync(int contactoId)
        {
            var cont = await _context.Contactos.FindAsync(contactoId);
            if (cont != null)
            {
                _context.Contactos.Remove(cont);
                await _context.SaveChangesAsync();
            }   
        }

        public async Task DeleteDomicilioAsync(int domicilioId)
        {
            var dom = await _context.Domicilios.FindAsync(domicilioId);
            if (dom != null)
            {
                _context.Domicilios.Remove(dom);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleados
                .Include(e => e.Area)
                .Include(e => e.Puesto)
                .Include(e => e.Turno)
                .Include(e => e.Estatus)
                .Include(e => e.Domicilio)
                .Include(e => e.ContactoEmergencia)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateAsync(Empleado empleado)
        {
            var existing = await _context.Empleados.FindAsync(empleado.Id);
            if (existing == null)
                return false;

            _context.Entry(existing).CurrentValues.SetValues(empleado);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<PagedResult<Empleado>> GetFilteredAsync(
           int? areaId, int? puestoId, int? estatusId, int page, int pageSize)
        {
            var query = _context.Empleados
                .Include(e => e.Area)
                .Include(e => e.Puesto)
                .Include(e => e.Turno)
                .Include(e => e.Estatus)
                .AsQueryable();

            //Filtros dinámicos
            if (areaId.HasValue && areaId.Value > 0)
                query = query.Where(e => e.AreaId == areaId.Value);

            if (puestoId.HasValue && puestoId.Value > 0)
                query = query.Where(e => e.PuestoId == puestoId.Value);

            if (estatusId.HasValue && estatusId.Value > 0)
                query = query.Where(e => e.EstatusId == estatusId.Value);

            //Conteo total para paginación
            var total = await query.CountAsync();

            //Aplicar paginación
            var empleados = await query
                .OrderBy(e => e.Nombre) // puedes cambiar el orden
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Empleado>
            {
                Items = empleados,
                Page = page,
                PageSize = pageSize,
                TotalItems = total
            };
        }
    }
}
