using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class BDContext : DbContext
    {
        public BDContext(DbContextOptions<BDContext> options) : base(options)
        {
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<CategoriaLibro> CategoriaLibros { get; set; }
        public DbSet<DetalleLibroAutor> DetalleLibroAutores { get; set; }
        public DbSet<DetallePrestamoLibro> DetallePrestamoLibros { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<NivelUsuario> NivelUsuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Autor>().ToTable("Autor");
            modelBuilder.Entity<CategoriaLibro>().ToTable("CategoriaLibro");
            modelBuilder.Entity<DetalleLibroAutor>().ToTable("DetalleLibroAutor");
            modelBuilder.Entity<DetallePrestamoLibro>().ToTable("DetallePrestamoLibro");
            modelBuilder.Entity<Libro>().ToTable("Libro");
            modelBuilder.Entity<NivelUsuario>().ToTable("NivelUsuario");
            modelBuilder.Entity<Prestamo>().ToTable("Prestamo");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
        }

    }
}
