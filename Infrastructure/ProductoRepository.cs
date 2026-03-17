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
    public class ProductoRepository : RepositoryBase<Producto>, IProductoRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductoRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        

        public async Task<List<Producto>> GetByNombreAsync(string? Nombre)
        {
            return await _dbContext.Productos
                .Where(p => p.Nombre.Contains(Nombre))
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<List<Producto>> GetByCategoryAsync(int? categoriaId)
        {
            return await _dbContext.Productos
                .Where(p => p.CategoriaId == categoriaId)
                .Include(p => p.Categoria)
                .ToListAsync();
        }
    }
}