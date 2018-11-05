using System.Collections.Generic;
using HoneyDo.Domain.Entities;
using HoneyDo.Infrastructure.Enumerations;
using HoneyDo.Infrastructure.Authentication;
using System.Linq;
using HoneyDo.Domain.Interfaces;
using System;

namespace HoneyDo.Infrastructure.Specifications
{
    public class FindAccount : ISpecification<Account>
    {
        private readonly Guid _accountId;
        
        public FindAccount(Guid accountId)
        {
            _accountId = accountId;
        }

        public IEnumerable<Account> Filter(IEnumerable<Account> items)
        {
            return items.Where(item => item.Id == _accountId);
        }
    }
}