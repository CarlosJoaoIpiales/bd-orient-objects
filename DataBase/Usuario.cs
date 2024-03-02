using DataBase.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase
{
    public class Usuario :IPersona
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NivelUsuarioId { get; set; }
        
        public string DNI { get; set; }
        string IPersona.Nombres { get; set; }
        string IPersona.Apellidos { get; set; }
        public string Sexo { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public string Password { get; set; }

        [ForeignKey(nameof(NivelUsuarioId))]
        public virtual NivelUsuario NivelUsuario { get; set; }
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}
