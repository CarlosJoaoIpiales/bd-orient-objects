using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Interfaces;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosPorAutorController : ControllerBase
    {
        private readonly BDContext _context;

        public LibrosPorAutorController(BDContext context)
        {
            _context = context;
        }

        [HttpGet("{nombre}/{apellido}")]
        public async Task<ActionResult<IEnumerable<DetalleLibros>>> GetLibrosPorNombreApellidoAutor(string nombre, string apellido)
        {
            var librosConDetalles = await _context.Libros
                .Join(_context.DetalleLibroAutores,
                    libro => libro.IdLibro,
                    detalle => detalle.LibroId,
                    (libro, detalle) => new { Libro = libro, Detalle = detalle })
                .Join(_context.Autores,
                    combined => combined.Detalle.AutorId,
                    autor => autor.Id,
                    (combined, autor) => new { combined.Libro, Autor = autor })
                .Where(result => result.Autor.Nombres.ToLower() == nombre.ToLower() && result.Autor.Apellidos.ToLower() == apellido.ToLower())
                .Join(_context.CategoriaLibros,
                    combined => combined.Libro.CategoriaId,
                    categoria => categoria.IdCategoriaLibro,
                    (combined, categoria) => new DetalleLibros
                    {
                        Id = combined.Libro.IdLibro,
                        Titulo = combined.Libro.Titulo,
                        Categoria = categoria.Descripcion,
                        Autores = _context.DetalleLibroAutores
                            .Where(detalle => detalle.LibroId == combined.Libro.IdLibro)
                            .Join(_context.Autores,
                                detalle => detalle.AutorId,
                                autor => autor.Id,
                                (detalle, autor) => autor.Nombres + " " + autor.Apellidos)
                            .ToList()
                    })
                .ToListAsync();

            if (librosConDetalles == null)
            {
                return NotFound();
            }

            return librosConDetalles;
        }
    }
}