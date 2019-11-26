using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Todos
{
    public class TodoById : ISpecification<Todo>
    {
        private readonly Guid _id;
        public TodoById(Guid id)
        {
            _id = id;
        }

        public Expression<Func<Todo, bool>> BuildExpression() => todo => todo.Id == _id;
    }
}
