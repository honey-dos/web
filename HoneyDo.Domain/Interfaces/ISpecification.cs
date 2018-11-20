using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HoneyDo.Domain.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> BuildExpression();
    }
}
