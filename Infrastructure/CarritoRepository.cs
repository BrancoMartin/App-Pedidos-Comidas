using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CarritoRepository : RepositoryBase<Carrito>, ICarritoRepository
    {
        private readonly AppDbContext _dbContext;
        public CarritoRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Carrito?> GetByUserIdAsync(int userId)
        {
            return await _dbContext.Carrito
                .Include(c => c.Items)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.UsuarioId == userId);
        }
        public override async Task<Carrito?> GetByIdAsync(int id)
        {
            return await _dbContext.Carrito
                .Include(c => c.Items)          
                    .ThenInclude(i => i.Producto) 
                .Include(c => c.Usuario)          
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Carrito>> GetAllCarritosAsync()
        {
            return await _dbContext.Carrito
                .Include(c => c.Usuario)
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .ToListAsync();
        }


    }
}
