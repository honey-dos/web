using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Values;
using Microsoft.EntityFrameworkCore;

namespace HoneyDo.Infrastructure.Context
{
    public class ContextRepository<T, TContext> : IRepository<T> where T : class where TContext : DbContext
    {
        private readonly DbSet<T> _dbset;
        private readonly Func<Task<int>> _save;

        public ContextRepository(TContext context)
        {
            _dbset = context.Set<T>();
            _save = () => { return context.SaveChangesAsync(); };
        }

        public async Task Add(T item)
        {
            await _dbset.AddAsync(item);
            await _save();
        }

        public Task<T> Find(ISpecification<T> spec, string load = "")
        {
            var query = _dbset.Where(spec.BuildExpression());
            if (!string.IsNullOrWhiteSpace(load))
            {
                query = query.Include(load);
            }
            return query.FirstOrDefaultAsync();
        }

        public Task<List<T>> Query(ISpecification<T> spec, Page page = null, string load = "")
        {
            IQueryable<T> queryable = _dbset.Where(spec.BuildExpression());
            if (page != null)
            {
                queryable = queryable.Skip((page.PageIndex - 1) * page.PageSize).Take(page.PageSize);
            }
            if (!string.IsNullOrWhiteSpace(load))
            {
                queryable = queryable.Include(load);
            }

            return queryable.ToListAsync();
        }

        public async Task<bool> Remove(T item)
        {
            var entityRemoved = _dbset.Remove(item);
            var numberOfChanges = await _save();
            return numberOfChanges == 1;
        }

        public async Task<bool> Update(T item)
        {
            var changeTracker = _dbset.Update(item);
            var numberOfChanges = await _save();
            return numberOfChanges == 1;
        }
    }
}
