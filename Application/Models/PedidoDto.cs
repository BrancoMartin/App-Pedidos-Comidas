using Domain.Enum;
using Domain.Entities;  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string TiempoEstimado { get; set; } = string.Empty;
        public decimal PrecioTotal { get; set; }
        public EstadoPedido EstadoPedido { get; set; }

        // Items del pedido
        public List<ItemPedidoDto> Items { get; set; } = new List<ItemPedidoDto>();
        public static PedidoDto CreatePedido(Pedido pedido)
        {
            var dto = new PedidoDto();
            dto.Id = pedido.Id;
            dto.UsuarioId = pedido.UsuarioId;
            dto.Direccion = pedido.Direccion;
            dto.TiempoEstimado = pedido.TiempoEstimado;
            dto.PrecioTotal = pedido.PrecioTotal;
            dto.EstadoPedido = pedido.EstadoPedido;
            dto.Items = pedido.ItemsPedido.Select(ItemPedidoDto.CreateItemPedido).ToList();
            return dto;
        }

        public static List<PedidoDto> CreateList(List<Pedido> pedidoList)
        {
            var dtoList = new List<PedidoDto>();
            foreach (var p in pedidoList)
            {
                dtoList.Add(CreatePedido(p));
            }
            return dtoList;
        }
    }

    public class ItemPedidoDto
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public static ItemPedidoDto CreateItemPedido(ItemPedido item)
        {
            var dto = new ItemPedidoDto();
            dto.Id = item.Id;
            dto.PedidoId = item.PedidoId;
            dto.ProductoId = item.ProductoId;
            dto.Cantidad = item.Cantidad;
            dto.PrecioUnitario = item.PrecioUnitario;
            return dto;
        }
    }


}
