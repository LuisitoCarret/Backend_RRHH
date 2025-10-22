namespace Modulo.Seguridad.Infrastructure.Persistence
{
    public class UsuarioEmpleadoRepository:IUsuarioEmpleadoRepository
    {
        private readonly SeguridadDbContext _context;

        public UsuarioEmpleadoRepository(SeguridadDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(UsuarioEmpleado relacion)
        {
            _context.UsuariosEmpleados.Add(relacion);
            await _context.SaveChangesAsync();
        }

        public async Task<long?> GetEmpleadoIdByUsuarioIdAsync(long usuarioId)
        {
            var rel = await _context.UsuariosEmpleados
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId);
            return rel?.EmpleadoId;
        }

        public async Task<UsuarioEmpleado?> GetByEmpleadoIdAsync(int empleadoId)
        {
            return await _context.UsuariosEmpleados
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmpleadoId == empleadoId);
        }

        public async Task<bool> HasOtherEmployeesAsync(long usuarioId)
        {
            return await _context.UsuariosEmpleados
        .AsNoTracking()
        .AnyAsync(ue => ue.UsuarioId == usuarioId);
        }

        public async Task DeleteAsync(long usuarioId, int empleadoId)
        {
            var relacion = await _context.UsuariosEmpleados
              .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.EmpleadoId == empleadoId);

            if (relacion != null)
            {
                _context.UsuariosEmpleados.Remove(relacion);
                await _context.SaveChangesAsync();
            }
        }

    }
}
