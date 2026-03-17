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
    public class CategoriaRepository : RepositoryBase<Categoria>,ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public CategoriaRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Categoria?> GetCategoryByName(string name)
        {
            return await _dbContext.Categorias
                .FirstOrDefaultAsync(c => c.NombreCategoria == name);
        }
    }
}
