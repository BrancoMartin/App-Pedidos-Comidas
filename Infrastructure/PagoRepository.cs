using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure
{
    public class PagoRepository : RepositoryBase<Pago>, IPagoRepository
    {
        private readonly AppDbContext _dbContext;

        public PagoRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Pago>> GetPagosByUserIdAsync(int userId)
        {
            return await _dbContext.Pagos
                .Include(p => p.Pedido)
                .Include(p => p.MetodoPago)
                .Where(p => p.Pedido.UsuarioId == userId)
                .ToListAsync();
        }

        public async Task<List<Pago>> GetPagosByPedidoIdAsync(int pedidoId)
        {
            return await _dbContext.Pagos
                .Include(p => p.MetodoPago)
                .Include(p => p.Pedido)
                .Where(p => p.PedidoId == pedidoId)
                .ToListAsync();
        }
    }
}
