using DataBase;
using DataBase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenarLibrosController : ControllerBase
    {
        private BDContext _context;

        public OrdenarLibrosController(BDContext context)
        {
            _context = context;

        }

        /*Recupera los libros ordenados alfabéticamente por título y apellido del autor. */

        [HttpGet("{apellido}")]
        public Task<List<DetalleLibros>> GetLibrosOrdenadosPorTituloYAutor(string apellido)
        {
            var librosConDetalles = _context.Libros
                .Join(_context.DetalleLibroAutores, libro => libro.IdLibro, detalle => detalle.LibroId, (libro, detalle) => new { libro, detalle })
                .Join(_context.Autores, ld => ld.detalle.AutorId, autor => autor.Id, (ld, autor) => new { ld.libro, ld.detalle, autor })
                .Where(lda => lda.autor.Apellidos.ToLower() == apellido.ToLower())
                .Join(_context.CategoriaLibros, lda => lda.libro.CategoriaId, categoria => categoria.IdCategoriaLibro, (lda, categoria) => new { lda.libro, lda.detalle, lda.autor, categoria })
                .OrderBy(lda => lda.libro.Titulo)
                .ThenBy(lda => lda.autor.Apellidos)
                .Select(lda => new DetalleLibros
                {
                    Id = lda.libro.IdLibro,
                    Titulo = lda.libro.Titulo,
                    Categoria = lda.categoria.Descripcion,
                    Autores = _context.DetalleLibroAutores
                        .Where(dl => dl.LibroId == lda.libro.IdLibro)
                        .Join(_context.Autores, dl => dl.AutorId, autor => autor.Id, (dl, autor) => autor.Nombres + " " + autor.Apellidos)
                        .ToList()
                })
                .ToList();
            if (!librosConDetalles.Any())
            {
                List<DetalleLibros> lista = new List<DetalleLibros>();
                lista.Add(new DetalleLibros
                {
                    // Puedes llenar los campos según tu lógica o dejarlos como predeterminados
                    Id = 0,
                    Titulo = "No se encuentra un autor con ese apellido",
                    Categoria = "Sin categoría",
                    Autores = new List<string>()
                });
                return Task.Run(() => lista);
            }

            return Task.Run(() => librosConDetalles);
        }

    }
}
