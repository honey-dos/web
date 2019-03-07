using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Infrastructure.Authentication;

namespace HoneyDo.Infrastructure.Specifications
{
    public class LoginsForAccount : ISpecification<Login>
    {
        private readonly Guid _accountId;
        public LoginsForAccount(Account account)
        {
            _accountId = account.Id;
        }

        public Expression<Func<Login, bool>> BuildExpression() => login => login.AccountId == _accountId;
    }
}