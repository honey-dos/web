using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Domain.Specifications.Groups
{
    public class GroupById : ISpecification<Group>
    {
        private readonly Guid _id;
        public GroupById(Guid id)
        {
            _id = id;
        }

        public Expression<Func<Group, bool>> BuildExpression() => group => group.Id == _id;
    }
}
