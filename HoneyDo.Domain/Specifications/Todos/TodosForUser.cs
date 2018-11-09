using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Todos
{
    public class TodosForUser : ISpecification<Todo>
    {
        public Expression<Func<Todo, bool>> BuildExpression() => todo => true;
    }
}
