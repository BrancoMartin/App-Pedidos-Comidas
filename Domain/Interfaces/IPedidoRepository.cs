using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPedidoRepository : IRepositoryBase<Pedido>
    {
        Task<List<Pedido>> GetByUserIdAsync(int userId);
        Task<List<Pedido>> GetAllPedidosAsync();
        Task<Pedido> GetPedidoByIdAsync(int id);
    }
}
