using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePrestamoLibroController : ControllerBase
    {
        private readonly BDContext _context;

        public DetallePrestamoLibroController(BDContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DetallePrestamoLibro>> GetDetallePrestamoLibros()
        {
            return _context.DetallePrestamoLibros.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<DetallePrestamoLibro> GetDetallePrestamoLibro(int id)
        {
            var detallePrestamoLibro = _context.DetallePrestamoLibros.Find(id);

            if (detallePrestamoLibro == null)
            {
                return NotFound();
            }

            return detallePrestamoLibro;
        }

        [HttpPost]
        public async Task<ActionResult<DetallePrestamoLibro>> PostDetallePrestamoLibro(DetallePrestamoLibro detallePrestamoLibro)
        {
            try
            {
                // Verificar si el libro y el préstamo existen en la base de datos
                if (!_context.Libros.Any(l => l.IdLibro == detallePrestamoLibro.LibroId) ||
                    !_context.Prestamos.Any(p => p.IdPrestamo == detallePrestamoLibro.PrestamoId))
                {
                    return BadRequest("El ID del libro o del préstamo no existe en la base de datos.");
                }

                _context.DetallePrestamoLibros.Add(detallePrestamoLibro);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDetallePrestamoLibro", new { id = detallePrestamoLibro.Id }, detallePrestamoLibro);
            }
            catch (Exception ex)
            {
                // Registrar la excepción para obtener más detalles
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetallePrestamoLibro(int id, DetallePrestamoLibro detallePrestamoLibro)
        {
            if (id != detallePrestamoLibro.Id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del detalle préstamo libro.");
            }

            try
            {
                // Verificar si el libro y el préstamo existen en la base de datos
                if (!_context.Libros.Any(l => l.IdLibro == detallePrestamoLibro.LibroId) ||
                    !_context.Prestamos.Any(p => p.IdPrestamo == detallePrestamoLibro.PrestamoId))
                {
                    return BadRequest("El ID del libro o del préstamo no existe en la base de datos.");
                }

                _context.Entry(detallePrestamoLibro).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Registrar la excepción para obtener más detalles
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetallePrestamoLibro(int id)
        {
            var detallePrestamoLibro = _context.DetallePrestamoLibros.Find(id);

            if (detallePrestamoLibro == null)
            {
                return NotFound();
            }

            // Verificar si el préstamo asociado está activo
            var prestamoAsociado = _context.Prestamos.Find(detallePrestamoLibro.PrestamoId);
            if (prestamoAsociado != null && prestamoAsociado.EstadoPrestamo)
            {
                return BadRequest("No se puede borrar el detalle porque el préstamo asociado está activo.");
            }

            _context.DetallePrestamoLibros.Remove(detallePrestamoLibro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
