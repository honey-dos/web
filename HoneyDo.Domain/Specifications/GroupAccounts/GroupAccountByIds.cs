using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.GroupAccounts
{
    public class GroupAccountByIds : ISpecification<GroupAccount>
    {
        private readonly Guid _groupId;
        private readonly Guid _accountId;
        public GroupAccountByIds(Guid groupId, Guid accountId)
        {
            _groupId = groupId;
            _accountId = accountId;
        }

        public Expression<Func<GroupAccount, bool>> BuildExpression() => ga =>
          ga.GroupId == _groupId
            && ga.AccountId == _accountId;
    }
}
