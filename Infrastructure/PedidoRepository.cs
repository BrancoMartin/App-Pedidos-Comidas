using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
    {
        private readonly AppDbContext _dbContext;
        public PedidoRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Pedido>> GetByUserIdAsync(int userId)
        {
            return await _dbContext.Pedidos
                .Where(p => p.UsuarioId == userId)
                .Include(p => p.Usuario)
                .Include(p => p.ItemsPedido)          
                .ThenInclude(ip => ip.Producto)  
                .ToListAsync();
        }

        public async Task<List<Pedido>> GetAllPedidosAsync()
        {
            return await _dbContext.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.ItemsPedido)
                .ThenInclude(ip => ip.Producto)
                .ToListAsync();
        }

        public virtual async Task<Pedido> GetPedidoByIdAsync(int id)
        {
            return await _dbContext.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.ItemsPedido)
                .ThenInclude(ip => ip.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<Pedido> GetByIdAsync(int pedidoId)
        {
            return await _dbContext.Pedidos
                .Include(p => p.ItemsPedido)
                .ThenInclude(ip => ip.Producto)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);
        }

        public override async Task<List<Pedido>> GetAllAsync()
        {
            return await _dbContext.Pedidos
                .Include(p => p.ItemsPedido)
                .ThenInclude(i => i.Producto)  
                .ToListAsync();
        }
    }
}
