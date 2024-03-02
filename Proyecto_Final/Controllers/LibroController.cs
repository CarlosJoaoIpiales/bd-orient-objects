// Importa los espacios de nombres necesarios
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBase;

[Route("api/[controller]")]
[ApiController]
public class LibroController : ControllerBase
{
    private BDContext _context;

    public LibroController(BDContext context)
    {
        _context = context;
    }

    // GET: api/Libro
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
    {
        return await _context.Libros.ToListAsync();
    }

    // GET: api/Libro/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> GetLibro(int id)
    {
        var libro = await _context.Libros.FindAsync(id);

        if (libro == null)
        {
            return NotFound();
        }

        return libro;
    }

    // POST: api/Libro
    [HttpPost]
    public async Task<ActionResult<Libro>> PostLibro(Libro libro)
    {
        if (VerificarISBNLibro(libro))
        {
            return BadRequest("El libro ya existe en la base de datos.");
        }
        _context.Libros.Add(libro);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetLibro", new { id = libro.IdLibro }, libro);
    }

    // PUT: api/Libro/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLibro(int id, Libro libro)
    {
        if (id != libro.IdLibro)
        {
            return BadRequest();
        }

        if (!VerificarISBNLibro(libro))
        {
            return BadRequest("El libro ya existe en la base de datos");
        }

        _context.Entry(libro).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LibroExists(id))
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

    // DELETE: api/Libro/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLibro(int id)
    {
        var ListaLibros = _context.Libros.ToList();
        var ListaDetalleLibroAutor = _context.DetalleLibroAutores.ToList();
        var libro = ListaLibros
            .FirstOrDefault(l => l.IdLibro == id);
        if (libro == null)
        {
            return NotFound();
        }

        // Validar si el libro está alquilado o prestado
        if (LibroEstaPrestado(id))
        {
            return BadRequest("No se puede borrar el libro porque está alquilado o prestado.");
        }

        // Obtener los detalles de autor asociados al libro
        var detallesAutor = ListaDetalleLibroAutor.Where(d => d.LibroId == id).ToList();

        // Eliminar los detalles de autor asociados al libro
        ListaDetalleLibroAutor.RemoveAll(d => detallesAutor.Contains(d));
        ListaLibros.Remove(libro);
        /*Cuando se realiza alguna modificación en estos objetos (como agregar, actualizar o eliminar registros), 
         * los cambios se almacenan inicialmente en la memoria en el contexto de Entity Framework y no se reflejan 
         * automáticamente en la base de datos. Para persistir esos cambios en la base de datos, es necesario llamar 
         * al método SaveChangesAsync() (o SaveChanges() en versiones síncronas).*/
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool LibroExists(int id)
    {
        return _context.Libros.Any(e => e.IdLibro == id);
    }

    private bool VerificarISBNLibro(Libro lib)
    {
        return _context.Libros.Any(l => l.ISBN == lib.ISBN);
    }

    private bool LibroEstaPrestado(int id)
    {
        // Verificar si el libro está alquilado o prestado
        return _context.DetallePrestamoLibros
            .Any(d => d.LibroId == id);
    }
}
