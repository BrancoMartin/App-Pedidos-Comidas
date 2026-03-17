using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Contraseña { get; set; }

        public string Telefono { get; set; }
        public RolUsuario Rol { get; set; }

        // Relaciones
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
    }
}
