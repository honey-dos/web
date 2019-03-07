
using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Accounts
{
    public class AccountById : ISpecification<Account>
    {
        private readonly Guid _accountId;
        public AccountById(Guid accountId)
        {
            _accountId = accountId;
        }

        public Expression<Func<Account, bool>> BuildExpression() => account => account.Id == _accountId;
    }
}
