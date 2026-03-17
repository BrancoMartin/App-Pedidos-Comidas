using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {

        private readonly AppDbContext _db_context;

        public RepositoryBase(AppDbContext dbContext)
        {
            _db_context = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _db_context.Set<T>().FindAsync(new object[] { id });
        }

        public virtual async Task<List<T>> GetAllAsync() { 
            return await _db_context.Set<T>().ToListAsync();
        }
        

        public virtual async Task<T> CreateAsync(T entity)
        {
            _db_context.Set<T>().Add(entity);
            await _db_context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _db_context.Set<T>().Update(entity);
            await _db_context.SaveChangesAsync();
        }


        public virtual async Task DeleteAsync(T entity)
        {
            _db_context.Set<T>().Remove(entity);
            await _db_context.SaveChangesAsync();
        }

    }
}
