using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using LogisticsSystem.Domain.Contracts.Repos;
using LogisticsSystem.Domain.Contracts.UOW;
using LogisticsSystem.Persistence.Contexts;
using LogisticsSystem.Persistence.Repos;

namespace LogisticsSystem.Persistence.UOW
{
    public class UnitOfWork(ApplicationDbContext context):IUnitOfWork
    {
        private readonly Dictionary<string,object> _Repos = [];
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T).Name;
            if (!_Repos.ContainsKey(type))
            {
                var Repo = new GenericRepository<T>(context);
                _Repos.Add(type, Repo);
            }
            return (IGenericRepository<T>)_Repos[type];
        }
        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
