using System.Collections.Generic;

namespace HoneyDo.Domain.Interfaces
{
    public interface ISpecification<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items);
    }
}

