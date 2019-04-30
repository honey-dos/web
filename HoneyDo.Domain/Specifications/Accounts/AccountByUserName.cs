using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Accounts
{
    public class AccountByUserName : ISpecification<Account>
    {
        private readonly string _userName;
        public AccountByUserName(string userName)
        {
            _userName = userName;
        }
        public Expression<Func<Account, bool>> BuildExpression() => account => account.NormalizedUserName == _userName;
    }
}