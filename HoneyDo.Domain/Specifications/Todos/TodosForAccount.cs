using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Todos
{
    public class TodosForAccount: ISpecification<Todo>
    {
        private readonly Guid _accountId;
        public TodosForAccount(Account account)
        {
            _accountId = account.Id;
        }

        public Expression<Func<Todo, bool>> BuildExpression() => todo => todo.CreatorId == _accountId;
    }
}
