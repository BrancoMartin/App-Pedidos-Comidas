using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPagoRepository : IRepositoryBase<Pago>
    {
        Task<List<Pago>> GetPagosByUserIdAsync(int userId);
        Task<List<Pago>> GetPagosByPedidoIdAsync(int pedidoId);
    }
}
