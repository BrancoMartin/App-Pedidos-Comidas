using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationPedidoDto
    {
        [Required(ErrorMessage = "El Id del usuario es requerido")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "La direccion del pedido requerida")]
        public string Direccion { get; set; } = string.Empty;
        public int CarritoId { get; set; }
}

    public class CreateItemPedidoDto
    {
        [Required(ErrorMessage = "El ID del producto es requerido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La Cantidad es requerida")]
        [Range(1, 30, ErrorMessage = "La cantidad debe estar entre 1 y 30")]
        public int Cantidad { get; set; }
    }

    public class CheckoutRequest
    {
        public int CarritoId { get; set; }
        public string DireccionEntrega { get; set; } = string.Empty;
    }

    // DTO para actualizar el estado del pedido
    public class UpdateEstadoPedidoDto
    {
        [Required(ErrorMessage = "El estado del pedido es requerido")]
        public EstadoPedido EstadoPedido { get; set; }
    }
}
