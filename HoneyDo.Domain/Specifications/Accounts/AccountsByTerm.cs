using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Accounts
{
    public class AccountsByTerm : ISpecification<Account>
    {
        private readonly string _term;
        private readonly string _normalizedTerm;
        public AccountsByTerm(string term)
        {
            _term = term;
            _normalizedTerm = term.ToUpper();
        }

        public Expression<Func<Account, bool>> BuildExpression() => account =>
            account.Name.ToUpper().Contains(_normalizedTerm)
            || account.NormalizedUserName.Contains(_normalizedTerm);
    }
}
