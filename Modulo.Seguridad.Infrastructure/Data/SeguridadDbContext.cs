namespace Modulo.Seguridad.Infrastructure.Data
{
    public class SeguridadDbContext:DbContext
    {
        public SeguridadDbContext(DbContextOptions<SeguridadDbContext> options): base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; } 
        public DbSet<Rol> Roles { get; set; } 
        public DbSet<UsuarioEmpleado> UsuariosEmpleados { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("seguridad");

            modelBuilder.Entity<UsuarioEmpleado>()
                .ToTable("usuarios_empleados")
                .HasKey(ue => new { ue.UsuarioId, ue.EmpleadoId });

            modelBuilder.Entity<UsuarioRol>(entity =>
            {
                entity.ToTable("usuarios_roles");

                entity.HasKey(ur => new { ur.UsuarioId, ur.RolId });

                entity.Property(ur => ur.UsuarioId)
                      .HasColumnName("usuario_id");

                entity.Property(ur => ur.RolId)
                      .HasColumnName("rol_id");

                entity.HasOne(ur => ur.Usuario)
                      .WithMany()
                      .HasForeignKey(ur => ur.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Rol)
                      .WithMany()
                      .HasForeignKey(ur => ur.RolId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");

                entity.HasKey(u => u.UsuarioId);

                entity.Property(u => u.UsuarioId)
                      .HasColumnName("usuario_id");

                entity.Property(u => u.Email)
                      .HasColumnName("email");

                entity.Property(u => u.PasswordHash)
                      .HasColumnName("password_hash");

                entity.Property(u => u.Nombre)
                      .HasColumnName("nombre");

                entity.Property(u => u.Estatus)
                      .HasColumnName("estatus");
            });

            // Configuración de roles
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(r => r.RolId);
                entity.Property(r => r.RolId).HasColumnName("rol_id");
                entity.Property(r => r.Nombre).HasColumnName("nombre");
                entity.Property(r => r.Slug).HasColumnName("slug");
            });

            // Configuración de usuarios_empleados
            modelBuilder.Entity<UsuarioEmpleado>(entity =>
            {
                entity.ToTable("usuarios_empleados");
                entity.HasKey(ue => new { ue.UsuarioId, ue.EmpleadoId });
                entity.Property(ue => ue.UsuarioId).HasColumnName("usuario_id");
                entity.Property(ue => ue.EmpleadoId).HasColumnName("empleado_id");
            });

        }
    }
}
