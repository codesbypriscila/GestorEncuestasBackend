using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Encuesta> Encuestas { get; set; }
        public DbSet<Pregunta> Preguntas { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Reporte> Reportes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Respuesta>()
                .HasOne(r => r.Encuesta)
                .WithMany()
                .HasForeignKey(r => r.EncuestaId);

            modelBuilder.Entity<Respuesta>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.UsuarioId);

            modelBuilder.Entity<Respuesta>()
                .HasOne(r => r.Pregunta)
                .WithMany()
                .HasForeignKey(r => r.PreguntaId);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Encuesta)
                .WithMany()
                .HasForeignKey(r => r.EncuestaId);

            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Usuario)
                .WithMany()
                .HasForeignKey(r => r.GeneradoPor);
        }
    }
}