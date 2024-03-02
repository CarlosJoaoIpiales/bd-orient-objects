using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Interfaces
{
    public class DetalleLibros
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public List<string> Autores { get; set; }

        public DetalleLibros(int id, string titulo, string categoria, List<string> autores)
        {
            Id = id;
            Titulo = titulo;
            Categoria = categoria;
            Autores = autores;
        }
        public DetalleLibros()
        {
            
        }

        public T Nuevo<T>(params object[] datos)
        {
            if (datos.Length != 4)
            {
                throw new ArgumentException("Se esperan exactamente 4 datos.");
            }

            if (typeof(T) != typeof(DetalleLibros))
            {
                throw new ArgumentException($"El tipo {typeof(T)} no es compatible con DetalleLibros.");
            }

            DetalleLibros detalle = new DetalleLibros();
            detalle.Id = (int)datos[0];
            detalle.Titulo = (string)datos[1];
            detalle.Categoria = (string)datos[2];
            detalle.Autores = (List<string>)datos[3];

            return (T)(object)detalle;
        }

        public IEnumerable<object> Modificar(IEnumerable<dynamic> datos)
        {
            // Implementación específica
            throw new System.NotImplementedException();
        }

        public IEnumerable<object> Eliminar(IEnumerable<dynamic> datos)
        {
            // Implementación específica
            throw new System.NotImplementedException();
        }

        
    }
}
