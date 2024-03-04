using DataBase.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class Autor: IPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }

        public virtual ICollection<DetalleLibroAutor>? DetalleLibroAutores { get; set; }
        
        
    }
}
