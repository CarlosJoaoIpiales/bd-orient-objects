using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class DetalleLibroAutor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LibroId { get; set; }
        public int AutorId { get; set; }

        [ForeignKey(nameof(LibroId))]
        public virtual Libro Libro { get; set; }
        [ForeignKey(nameof(AutorId))]
        public virtual Autor Autor { get; set; }
        
    }
}


