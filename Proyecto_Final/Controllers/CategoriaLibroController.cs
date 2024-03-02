using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;

namespace Proyecto_Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaLibroController : ControllerBase
    {
        private BDContext _context;

        public CategoriaLibroController(BDContext context)
        {
            _context = context;
        }

        // GET: api/CategoriaLibro
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaLibro>> GetCategoriasLibro()
        {
            return _context.CategoriaLibros.ToList();
        }

        // GET: api/CategoriaLibro/5
        [HttpGet("{id}")]
        public ActionResult<CategoriaLibro> GetCategoriaLibro(int id)
        {
            var categoriaLibro = _context.CategoriaLibros.Find(id);

            if (categoriaLibro == null)
            {
                return NotFound();
            }

            return categoriaLibro;
        }

        // POST: api/CategoriaLibro
        [HttpPost]
        public async Task<ActionResult<CategoriaLibro>> PostCategoriaLibro(CategoriaLibro categoriaLibro)
        {
            try
            {
                // Verificar si la categoría ya existe en la base de datos
                if (ExisteCategoria(categoriaLibro.Descripcion))
                {
                    return BadRequest("La categoría ya existe en la base de datos.");
                }

                _context.CategoriaLibros.Add(categoriaLibro);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCategoriaLibro", new { id = categoriaLibro.IdCategoriaLibro }, categoriaLibro);
            }
            catch (Exception ex)
            {
                // Registrar la excepción para obtener más detalles
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // PUT: api/CategoriaLibro/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoriaLibro(int id, CategoriaLibro categoriaLibro)
        {
            if (id != categoriaLibro.IdCategoriaLibro)
            {
                return BadRequest("El ID proporcionado no coincide con el ID de la categoría.");
            }

            try
            {
                // Verificar si la categoría ya existe en la base de datos (excepto para la misma categoría)
                if (ExisteCategoria(categoriaLibro.Descripcion))
                {
                    return BadRequest("La categoría ya existe en la base de datos.");
                }

                _context.Entry(categoriaLibro).State = EntityState.Modified;
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

        // DELETE: api/CategoriaLibro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaLibro(int id)
        {
            var ListaLibros = _context.Libros.ToList();
            var categoriaLibro = _context.CategoriaLibros.Find(id);

            if (categoriaLibro == null)
            {
                return NotFound();
            }

            // Verificar si la categoría está asociada a algún libro
            if (ListaLibros.Any(l => l.CategoriaId == id))
            {
                return BadRequest("No se puede borrar la categoría porque está asociada a uno o más libros.");
            }

            _context.CategoriaLibros.Remove(categoriaLibro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExisteCategoria(string nombre)
        {
            return _context.CategoriaLibros.Any(c => c.Descripcion == nombre);
        }
    }
}
