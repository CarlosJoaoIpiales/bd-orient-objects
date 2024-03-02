using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly BDContext _context;

        public UsuarioController(BDContext context)
        {
            _context = context;
        }

        // GET: api/Usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuario
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Verificar si el DNI ya está registrado
            if (UsuarioDniExists(usuario.DNI))
            {
                return BadRequest("El DNI ya está registrado en la base de datos.");
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // PUT: api/Usuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest("El ID proporcionado no coincide con el ID del usuario.");
            }

            // Verificar si el usuario existe
            if (!UsuarioExists(id))
            {
                return NotFound();
            }

            // Verificar si el DNI ya está registrado
            if (UsuarioDniExists(usuario.DNI))
            {
                return BadRequest("El DNI ya está registrado en la base de datos.");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // DELETE: api/Usuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Verificar si el usuario tiene libros prestados
            if (UsuarioTieneLibrosPrestados(id))
            {
                return BadRequest("No se puede borrar el usuario porque tiene libros prestados.");
            }

            // Eliminar registros de préstamos asociados al usuario
            EliminarPrestamosUsuario(id);

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        private bool UsuarioDniExists(string dni)
        {
            return _context.Usuarios.Any(u => u.DNI == dni);
        }

        private bool UsuarioTieneLibrosPrestados(int id)
        {
            return _context.Prestamos.Any(p => p.UsuarioId == id && p.EstadoPrestamo == true);
        }

        private void EliminarPrestamosUsuario(int usuarioId)
        {
            var prestamosUsuario = _context.Prestamos.Where(p => p.UsuarioId == usuarioId).ToList();
            _context.Prestamos.RemoveRange(prestamosUsuario);
            _context.SaveChanges();
        }
    }
}
