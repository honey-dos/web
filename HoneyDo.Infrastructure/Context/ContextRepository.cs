using System;
using System.Collections.Generic;
using System.Linq;
using HoneyDo.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HoneyDo.Infrastructure.Context
{
    public class ContextRepository<T, TContext> : IRepository<T> where T : class where TContext : DbContext
    {
        private readonly DbSet<T> _dbset;
        private readonly Action _save;

        public ContextRepository(TContext context)
        {
            _dbset = context.Set<T>();
            _save = () => { context.SaveChanges(); };
        }

        public void Add(T item)
        {
            _dbset.Add(item);
            _save();
        }

        public T Find(ISpecification<T> spec)
        {
            return spec.Filter(_dbset).FirstOrDefault();
        }

        public IEnumerable<T> Query(ISpecification<T> spec)
        {
            return spec.Filter(_dbset);
        }

        public bool Remove(T item)
        {
            var entityRemoved = _dbset.Remove(item);
            _save();
            return entityRemoved != null;
        }

        public void Update(T item)
        {
            var changeTracker = _dbset.Update(item);
            _save();
        }
    }
}
