using DataBase.Interfaces;
using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleLibrosApellidoController : ControllerBase
    {
        private BDContext _context;

        public DetalleLibrosApellidoController(BDContext context)
        {
            _context = context;

        }

        [HttpGet("{apellido}")]
        public Task<List<DetalleLibros>> GetLibrosPorApellidoAutor(string apellido)
        {
            var ListaLibros = _context.Libros.ToList();
            var ListaDetalleLibroAutores = _context.DetalleLibroAutores.ToList();
            var ListaAutores = _context.Autores.ToList();
            var ListaCategorias = _context.CategoriaLibros.ToList();
            var librosConDetalles = (from libro in ListaLibros
                                     join detalle in ListaDetalleLibroAutores on libro.IdLibro equals detalle.LibroId
                                     join autor in ListaAutores on detalle.AutorId equals autor.Id
                                     where autor.Apellidos.ToLower() == apellido.ToLower()
                                     join categoria in ListaCategorias on libro.CategoriaId equals categoria.IdCategoriaLibro
                                     select new DetalleLibros
                                     {
                                         Id = libro.IdLibro,
                                         Titulo = libro.Titulo,
                                         Categoria = categoria.Descripcion,
                                         Autores = (from detalle in ListaDetalleLibroAutores
                                                    join autor in ListaAutores on detalle.AutorId equals autor.Id
                                                    where detalle.LibroId == libro.IdLibro
                                                    select autor.Nombres + " " + autor.Apellidos).ToList()
                                     }).ToList();

            return Task.Run(() => librosConDetalles);
        }



    }
}
