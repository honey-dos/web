using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.GroupAccounts
{
    public class GroupAccountForGroup: ISpecification<GroupAccount>
    {
        private readonly Guid _groupId;
        public GroupAccountForGroup(Group group)
        {
            _groupId = group.Id;
        }

        public Expression<Func<GroupAccount, bool>> BuildExpression() => ga => ga.GroupId == _groupId;
    }
}
