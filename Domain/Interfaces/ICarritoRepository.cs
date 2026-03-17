using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICarritoRepository : IRepositoryBase<Carrito>
    {
        Task<Carrito?> GetByUserIdAsync(int userId);
        Task<List<Carrito>> GetAllCarritosAsync();
    }
}
