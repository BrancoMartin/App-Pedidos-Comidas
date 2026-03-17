using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string NombreCategoria { get; set; } = null!;

        // Relaciones
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
