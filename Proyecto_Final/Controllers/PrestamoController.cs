using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private BDContext _context;

        public PrestamoController(BDContext context)
        {
            _context = context;
        }

        // GET: api/Prestamo
        [HttpGet]
        public ActionResult<IEnumerable<Prestamo>> GetPrestamos()
        {
            return _context.Prestamos.ToList();
        }

        // GET: api/Prestamo/5
        [HttpGet("{id}")]
        public ActionResult<Prestamo> GetPrestamo(int id)
        {
            var prestamo = _context.Prestamos.Find(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return prestamo;
        }

        // POST: api/Prestamo
        [HttpPost]
        public async Task<ActionResult<Prestamo>> PostPrestamo(Prestamo prestamo)
        {
            try
            {
                if (!VerificarUsuario(prestamo.UsuarioId))
                {
                    return BadRequest("El usuario especificado no existe en la base de datos.");
                }

                // Verificar si existe una fecha y hora de prestamos
                if (prestamo.FechaPrestamo == null && prestamo.HoraPrestamo == null)
                {
                    return BadRequest("La fecha y hora de préstamo son obligatorias.");
                }

                // Validar la fecha de devolución
                if (prestamo.FechaPrestamo < DateTime.Now)
                {
                    return BadRequest("La fecha de prestamo no puede ser menor a la fecha actual.");
                }

                // Validar que cuando el prestamo este activo, no se ingrese la fecha y hora de devolucion
                if (prestamo.EstadoPrestamo == true)
                {
                    if (prestamo.FechaDevolucion != null || prestamo.HoraDevolucion != null)
                    {
                        return BadRequest("La fecha de prestamo no puede ser menor a la fecha actual.");
                    }
                }

                // Validar que no se puedan crear prestamos inactivos

                if (prestamo.EstadoPrestamo == false)
                {
                    return BadRequest("No se puede crear un prestamo inactivo.");
                }

                _context.Prestamos.Add(prestamo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPrestamo", new { id = prestamo.IdPrestamo }, prestamo);
            }
            catch (Exception ex)
            {
                // Log de la excepción para obtener más detalles
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/Prestamo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrestamo(int id, Prestamo prestamo)
        {
            if (id != prestamo.IdPrestamo)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del préstamo.");
            }

            try
            {
                // Validar si el usuario existe
                var usuarioExistente = await _context.Usuarios.FindAsync(prestamo.UsuarioId);
                if (usuarioExistente == null)
                {
                    return BadRequest("El usuario especificado no existe en la base de datos.");
                }

                // Verificar si existe una fecha y hora de prestamos
                if (prestamo.FechaPrestamo == null && prestamo.HoraPrestamo == null)
                {
                    return BadRequest("La fecha y hora de préstamo son obligatorias.");
                }

                // Validar la fecha de devolución
                if (prestamo.FechaDevolucion < DateTime.Now)
                {
                    return BadRequest("La fecha de devolución no puede ser menor a la fecha actual.");
                }

                _context.Entry(prestamo).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log de la excepción para obtener más detalles
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // DELETE: api/Prestamo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrestamo(int id)
        {
            var prestamo = _context.Prestamos.Find(id);

            if (prestamo == null)
            {
                return NotFound();
            }

            if (prestamo.EstadoPrestamo == true)
            {
                return BadRequest("No se puede eliminar este prestamo ya que se encuentra activo.");
            }

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VerificarUsuario(int id)
        {
            return _context.Usuarios.Any(u => u.Id == id);  
        }
    }
}
