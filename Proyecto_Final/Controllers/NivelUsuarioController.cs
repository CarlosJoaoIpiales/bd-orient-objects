using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NivelUsuarioController : ControllerBase
    {
        private readonly BDContext _context;

        public NivelUsuarioController(BDContext context)
        {
            _context = context;
        }

        // GET: api/NivelUsuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NivelUsuario>>> GetNivelesUsuario()
        {
            return await _context.NivelUsuarios.ToListAsync();
        }

        // GET: api/NivelUsuario/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NivelUsuario>> GetNivelUsuario(int id)
        {
            var nivelUsuario = await _context.NivelUsuarios.FindAsync(id);

            if (nivelUsuario == null)
            {
                return NotFound();
            }

            return nivelUsuario;
        }

        // POST: api/NivelUsuario
        [HttpPost]
        public async Task<ActionResult<NivelUsuario>> PostNivelUsuario(NivelUsuario nivelUsuario)
        {
            // Verificar si el nivel ya existe
            if (NivelUsuarioExists(nivelUsuario))
            {
                return BadRequest("El nivel de usuario ya existe.");
            }

            _context.NivelUsuarios.Add(nivelUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNivelUsuario", new { id = nivelUsuario.IdNivelUsuario }, nivelUsuario);
        }

        // PUT: api/NivelUsuario/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNivelUsuario(int id, NivelUsuario nivelUsuario)
        {

            // Verificar si el nivel de usuario existe
            if (NivelUsuarioExists(nivelUsuario))
            {
                return BadRequest("El nivel de usuario ya existe.");
            }

            if (id != nivelUsuario.IdNivelUsuario)
            {
                return BadRequest("Los ids no coinciden.");
            }

            _context.Entry(nivelUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExistNivel(id))
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

        // DELETE: api/NivelUsuario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNivelUsuario(int id)
        {
            var nivelUsuario = await _context.NivelUsuarios.FindAsync(id);

            if (nivelUsuario == null)
            {
                return NotFound();
            }

            if (NivelAsociado(id))
            {
                return BadRequest("No se puede eliminar el nivel porque ya se encuentra asociado a uno o mas usuarios.");
            }

            _context.NivelUsuarios.Remove(nivelUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NivelUsuarioExists(NivelUsuario nivel)
        {
            return _context.NivelUsuarios.Any(n => n.Descripcion == nivel.Descripcion);
        }

        private bool ExistNivel(int id)
        {
            return _context.NivelUsuarios.Any(n => n.IdNivelUsuario == id);
        }

        private bool NivelAsociado(int id)
        {
            return _context.Usuarios.Any(u => u.NivelUsuarioId == id);
        }
    }
}
