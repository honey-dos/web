using System.Collections.Generic;
using System.Threading.Tasks;

namespace HoneyDo.Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task Add(T item);
        Task<bool> Update(T item);
        Task<bool> Remove(T item);
        Task<T> Find(ISpecification<T> spec);
        Task<List<T>> Query(ISpecification<T> spec);
    }
}
