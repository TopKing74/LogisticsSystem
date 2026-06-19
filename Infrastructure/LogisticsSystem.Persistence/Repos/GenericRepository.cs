using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogisticsSystem.Domain.Contracts.Repos;
using LogisticsSystem.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Persistence.Repos
{
    public class GenericRepository<T>(ApplicationDbContext context): IGenericRepository<T> where T : class
    {
        public async Task<T?> GetByIdAsync(int id) =>await context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllAsync() => await context.Set<T>().ToListAsync();

        public async Task AddAsync(T entity) => await context.Set<T>().AddAsync(entity);

        public void Update(T entity) => context.Set<T>().Update(entity);

        public void Delete(T entity) => context.Set<T>().Remove(entity);
    }
}
