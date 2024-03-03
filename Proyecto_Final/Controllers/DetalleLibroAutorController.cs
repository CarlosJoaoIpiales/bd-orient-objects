using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleLibroAutorController : ControllerBase
    {
        private BDContext _context;

        public DetalleLibroAutorController(BDContext context)
        {
            _context = context;
        }

        // GET: api/DetalleLibroAutor
        [HttpGet]
        public ActionResult<IEnumerable<DetalleLibroAutor>> GetDetallesLibroAutor()
        {
            return _context.DetalleLibroAutores.ToList();
        }

        // GET: api/DetalleLibroAutor/5
        [HttpGet("{id}")]
        public ActionResult<DetalleLibroAutor> GetDetalleLibroAutor(int id)
        {
            var detalleLibroAutor = _context.DetalleLibroAutores.Find(id);

            if (detalleLibroAutor == null)
            {
                return NotFound();
            }

            return detalleLibroAutor;
        }

        // POST: api/DetalleLibroAutor
        [HttpPost]
        public async Task<ActionResult<DetalleLibroAutor>> PostDetalleLibroAutor(DetalleLibroAutor detalleLibroAutor)
        {
            try
            {
                // Verificar si el libro y el autor existen en la base de datos
                if (!LibroExists(detalleLibroAutor.LibroId) || !AutorExists(detalleLibroAutor.AutorId))
                {
                    return BadRequest("El libro o el autor no existen en la base de datos.");
                }

                _context.DetalleLibroAutores.Add(detalleLibroAutor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDetalleLibroAutor", new { id = detalleLibroAutor.Id }, detalleLibroAutor);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/DetalleLibroAutor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleLibroAutor(int id, DetalleLibroAutor detalleLibroAutor)
        {
            if (id != detalleLibroAutor.Id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del detalle libro autor.");
            }

            try
            {
                // Verificar si el libro y el autor existen en la base de datos
                if (!LibroExists(detalleLibroAutor.LibroId) || !AutorExists(detalleLibroAutor.AutorId))
                {
                    return BadRequest("El libro o el autor no existen en la base de datos.");
                }

                _context.Entry(detalleLibroAutor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/DetalleLibroAutor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleLibroAutor(int id)
        {
            var detalleLibroAutor = _context.DetalleLibroAutores.Find(id);

            if (detalleLibroAutor == null)
            {
                return NotFound();
            }

            _context.DetalleLibroAutores.Remove(detalleLibroAutor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LibroExists(int libroId)
        {
            return _context.Libros.Any(l => l.IdLibro == libroId);
        }

        private bool AutorExists(int autorId)
        {
            return _context.Autores.Any(a => a.Id == autorId);
        }
    }
}
