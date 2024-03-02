using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private BDContext _context;

        public AutorController(BDContext context)
        {
            _context = context;
        }

        // GET: api/Autor
        [HttpGet]
        public ActionResult<IEnumerable<Autor>> GetAutores()
        {
            return _context.Autores.ToList();
        }

        // GET: api/Autor/5
        [HttpGet("{id}")]
        public ActionResult<Autor> GetAutor(int id)
        {
            var autor = _context.Autores.Find(id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        // POST: api/Autor
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor)
        {
            var ListaAutores = _context.Autores.ToList();
            var autor1 = ListaAutores.FirstOrDefault(a => a.Id == autor.Id);
            // Verificar si el autor ya existe en la base de datos
            if (AutorExisteDB(autor))
            {
                return BadRequest("El autor ya existe en la base de datos.");
            }

            _context.Autores.Add(autor); // Agregar el nuevo autor al DbSet
            await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos

            return CreatedAtAction("GetAutor", new { id = autor.Id }, autor);
        }

        // PUT: api/Autor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del autor.");
            }

            _context.Entry(autor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Autor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var ListaAutores = _context.Autores.ToList();
            var autor = ListaAutores.FirstOrDefault(a => a.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            // Verificar si el autor está asociado a algún libro
            if (AutorAsociadoLibro(id))
            {
                return BadRequest("No se puede borrar el autor porque está asociado a uno o más libros.");
            }

            _context.Autores.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.Id == id);
        }

        private bool AutorExisteDB(Autor autor)
        {
            return _context.Autores.Any(a => (a.Nombres == autor.Nombres && a.Apellidos == autor.Apellidos) || a.Id == autor.Id);
        }

        private bool AutorAsociadoLibro(int id)
        {
            return _context.DetalleLibroAutores.Any(a => a.AutorId == id);
        }

    }
}
