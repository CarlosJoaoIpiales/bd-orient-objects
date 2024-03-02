using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class DetallePrestamoLibro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LibroId { get; set; }
        
        public int PrestamoId { get; set; }

        [ForeignKey(nameof(LibroId))]
        public virtual Libro Libro { get; set; }
        [ForeignKey(nameof(PrestamoId))]
        public virtual Prestamo Prestamo { get; set; }
    }
}
