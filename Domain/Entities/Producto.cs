using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        // Relaciones
        public Categoria Categoria { get; set; } = null!;
        public ICollection<ItemPedido> ItemsPedido { get; set; } = new List<ItemPedido>();
        public ICollection<ItemCarrito> ItemsCarrito { get; set; } = new List<ItemCarrito>();
    }
}
