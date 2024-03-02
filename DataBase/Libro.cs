using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Interfaces;

namespace DataBase
{
    public class Libro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdLibro { get; set; }
        public int CategoriaId { get; set; }
        
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string ISBN { get; set; }
        public int NumeroPaginas { get; set; }
        public string Descripcion { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        public virtual CategoriaLibro Categoria { get; set; }
        public virtual ICollection<DetalleLibroAutor> DetalleLibroAutores { get; set; }
        public virtual ICollection<DetallePrestamoLibro> DetallePrestamoLibros { get; set; }
    }
}
