using System.Collections.Generic;
using HoneyDo.Domain.Entities;
using HoneyDo.Infrastructure.Authentication;
using System.Linq;
using HoneyDo.Domain.Interfaces;
using System;
using System.Linq.Expressions;

namespace HoneyDo.Infrastructure.Specifications
{
    public class LoginByProvider : ISpecification<Login>
    {
        private readonly string _provider;
        private readonly string _providerId;

        public LoginByProvider(string provider, string providerId)
        {
            _provider = provider;
            _providerId = providerId;
        }

        public Expression<Func<Login, bool>> BuildExpression() => login => login.Provider == _provider && login.ProviderKey == _providerId;
    }
}
