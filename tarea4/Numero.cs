using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tarea4
{
    public class Numero
    {
        [Key]
        public int NumeroId { get; set; }

        public int Orden { get; set; }

        public int Num { get; set; }

        // Clave foránea
        public int ProductoId { get; set; }

        // Navegación: Un número pertenece a un producto
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }
    }
}