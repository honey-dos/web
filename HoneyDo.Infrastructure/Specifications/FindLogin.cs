using System.Collections.Generic;
using HoneyDo.Domain.Entities;
using HoneyDo.Infrastructure.Enumerations;
using HoneyDo.Infrastructure.Authentication;
using System.Linq;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Infrastructure.Specifications
{
    public class FindLogin : ISpecification<Login>
    {
        private readonly string _provider;
        private readonly string _providerId;
        
        public FindLogin(Providers provider, string providerId)
        {
            _provider = provider.ToString();
            _providerId = providerId;
        }

        public IEnumerable<Login> Filter(IEnumerable<Login> items)
        {
            return items.Where(login => login.Provider == _provider && login.ProviderId == _providerId);
        }
    }
}