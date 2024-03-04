using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClasificacionLibrosController : ControllerBase
    {
        private BDContext _context;

        public ClasificacionLibrosController(BDContext context)
        {
            _context = context;

        }

        [HttpGet("{genero}")]
        public Task<List<string>> GetLibrosPorGenero(string genero)
        {
            var ListaLibros = _context.Libros.ToList();
            var ListaCategorias = _context.CategoriaLibros.ToList();
            var librosPorGenero = (from libro in ListaLibros
                                   join categoria in ListaCategorias
                                   on libro.CategoriaId equals categoria.IdCategoriaLibro
                                   where categoria.Descripcion.ToLower() == genero.ToLower()
                                   select new
                                   {
                                       id = libro.IdLibro,
                                       nombre = libro.Titulo,
                                       paginas = libro.NumeroPaginas,
                                       genero = categoria.Descripcion

                                   }).ToList();
            
            List<string> libros = new List<string>();
            if (!librosPorGenero.Any())
            {
                libros.Add("No se ha encontrado ningun libro con es genero");
                return Task.Run(() => libros);
            }
            foreach (var libro in librosPorGenero)
            {
                libros.Add(libro.nombre.ToString());
                libros.Add(libro.paginas.ToString());
                libros.Add(libro.genero.ToString());
            }
            return Task.Run(() => libros);
        }
    }
}
