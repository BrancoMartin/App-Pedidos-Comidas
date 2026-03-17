using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Direccion { get; set; }
        public string TiempoEstimado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioTotal { get; set; }
        public EstadoPedido EstadoPedido { get; set; } 

        // Relaciones
        public Usuario Usuario { get; set; } 
        public ICollection<ItemPedido> ItemsPedido { get; set; } = new List<ItemPedido>();
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
