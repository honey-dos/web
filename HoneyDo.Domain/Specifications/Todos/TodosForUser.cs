using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Todos
{
    public class TodosForUser : ISpecification<Todo>
    {
        private readonly Guid _ownerId;
        public TodosForUser(Account account)
        {
            _ownerId = account.Id;
        }

        public Expression<Func<Todo, bool>> BuildExpression() => todo => todo.CreatorId == _ownerId;
    }
}
