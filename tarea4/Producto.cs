using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace tarea4
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }

        [Required]
        public string Nombre { get; set; }

        public DateTime FechaHora { get; set; }

        // Navegación: Un producto tiene muchos números
        public virtual ICollection<Numero> Numeros { get; set; }

        public Producto()
        {
            Numeros = new List<Numero>();
            FechaHora = DateTime.Now;
        }
    }
}