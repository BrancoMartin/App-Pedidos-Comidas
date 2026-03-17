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
    public class MetodoPagoRepository : RepositoryBase<MetodoPago>, IMetodoPagoRepository
    {
        private readonly AppDbContext _dbContext;

        public MetodoPagoRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

       
        public async Task<List<MetodoPago?>> GetMetodoPagoByUserIdAsync(int userId)
        {
            return await _dbContext.MetodoPagos
                .Where(mp => mp.UsuarioId == userId)
                .ToListAsync();
        }


    }
}
