using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.Models.Request;
using Domain.Enum;

namespace Application.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDto> GetPedidoById(int id);
        Task<IEnumerable<PedidoDto>> GetAllPedidos();
        Task<IEnumerable<PedidoDto>> GetPedidosByEstado(EstadoPedido estado);
        Task<PedidoDto> CreatePedido(CreationPedidoDto creationPedidoDto);
        Task UpdateEstadoPedido(int id, EstadoPedido nuevoEstado);
        Task DeletePedido(int id);
        Task <IEnumerable<PedidoDto>> GetPedidosByUsuarioId(int usuarioId);
        Task<PedidoDto> AddItemToPedido(int pedidoId, CreateItemPedidoDto itemPedido);

    }
}
