namespace Modulo.Empleados.Infrastructure.Data
{
    public class EmpleadosDbContext:DbContext
    {
        public EmpleadosDbContext(DbContextOptions<EmpleadosDbContext> options) : base(options) { }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<DomicilioEmpleado> Domicilios { get; set; }
        public DbSet<ContactoEmergencia> Contactos { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<EstatusEmpleado> Estatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("empleados");

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.ToTable("empleados");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
                entity.Property(e => e.AreaId).HasColumnName("area_id");
                entity.Property(e => e.PuestoId).HasColumnName("puesto_id");
                entity.Property(e => e.TurnoId).HasColumnName("turno_id");
                entity.Property(e => e.EstatusId).HasColumnName("estatus_id");

                entity.HasOne(e => e.Domicilio)
         .WithOne(d => d.Empleado)
         .HasForeignKey<DomicilioEmpleado>(d => d.EmpleadoId)
         .OnDelete(DeleteBehavior.Cascade);

                // 🔹 Relación 1:1 con Contacto de emergencia
                entity.HasOne(e => e.ContactoEmergencia)
                      .WithOne(c => c.Empleado)
                      .HasForeignKey<ContactoEmergencia>(c => c.EmpleadoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<DomicilioEmpleado>(entity =>
            {
                entity.ToTable("domicilios_empleado");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Calle).HasColumnName("calle");
                entity.Property(e => e.Numero).HasColumnName("numero");
                entity.Property(e => e.Colonia).HasColumnName("colonia");
                entity.Property(e => e.Ciudad).HasColumnName("ciudad");
                entity.Property(e => e.Estado).HasColumnName("estado");
                entity.Property(e => e.CodigoPostal).HasColumnName("codigo_postal");
                entity.Property(e => e.EmpleadoId).HasColumnName("empleado_id");
            });

            modelBuilder.Entity<ContactoEmergencia>(entity =>
            {
                entity.ToTable("contactos_emergencia");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Parentesco).HasColumnName("parentesco");
                entity.Property(e => e.Telefono).HasColumnName("telefono");
                entity.Property(e => e.EmpleadoId).HasColumnName("empleado_id");
            });

            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("areas");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            });

             modelBuilder.Entity<Puesto>(entity =>
            {
                entity.ToTable("puestos");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.Descripcion).HasColumnName("descripcion");
                entity.Property(e => e.AreaId).HasColumnName("area_id");
            });

            modelBuilder.Entity<Turno>(entity =>
            {
                entity.ToTable("turnos");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
                entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
                entity.Property(e => e.ToleranciaMinutos).HasColumnName("tolerancia_minutos");
            });

            modelBuilder.Entity<EstatusEmpleado>(entity =>
            {
                entity.ToTable("estatus_empleado");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Nombre).HasColumnName("nombre");
            });

        }
     }
}
