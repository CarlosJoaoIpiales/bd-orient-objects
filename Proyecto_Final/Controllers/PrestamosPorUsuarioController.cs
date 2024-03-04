using System;
using System.Linq;
using System.Threading.Tasks;
using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosPorUsuarioController : ControllerBase
    {
        private readonly BDContext _context;

        public PrestamosPorUsuarioController(BDContext context)
        {
            _context = context;
        }

        [HttpGet("{usuarioId}")]
        public async Task<ActionResult> GetPrestamosPorUsuario(int usuarioId)
        {
            try
            {
                var prestamosPorUsuario = await _context.Prestamos
                    .Where(p => p.UsuarioId == usuarioId)
                    .Select(p => new
                    {
                        p.IdPrestamo,
                        p.UsuarioId,
                        p.EstadoPrestamo,
                        p.FechaPrestamo,
                        p.FechaDevolucion
                    })
                    .ToListAsync();

                if (prestamosPorUsuario == null || !prestamosPorUsuario.Any())
                {
                    return NotFound("No se encontraron préstamos para el usuario especificado.");
                }

                return Ok(prestamosPorUsuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}