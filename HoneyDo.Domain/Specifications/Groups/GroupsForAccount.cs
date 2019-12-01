using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Groups
{
    public class GroupsForAccount : ISpecification<Group>
    {
        private readonly Guid _accountId;
        public GroupsForAccount(Account account)
        {
            _accountId = account.Id;
        }

        public Expression<Func<Group, bool>> BuildExpression() => group => group.CreatorId == _accountId;
    }
}
