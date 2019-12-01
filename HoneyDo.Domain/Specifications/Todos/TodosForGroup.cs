using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Todos
{
    public class TodosForGroup : ISpecification<Todo>
    {
        private readonly Guid _groupId;
        public TodosForGroup(Group group)
        {
            _groupId = group.Id;
        }

        public Expression<Func<Todo, bool>> BuildExpression() => 
          todo => todo.GroupId.HasValue && todo.GroupId.Value == _groupId;
    }
}
