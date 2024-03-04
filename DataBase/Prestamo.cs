using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataBase
{
    public class Prestamo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPrestamo { get; set; }
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        public bool EstadoPrestamo { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime HoraPrestamo { get; set; }

        public DateTime? FechaDevolucion { get; set; }

        // Se almacena solo la hora utilizando TimeSpan para almacenar solo la hora
        public DateTime? HoraDevolucion { get; set; }

        
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<DetallePrestamoLibro> DetallePrestamoLibros { get; set; }
    }
}
