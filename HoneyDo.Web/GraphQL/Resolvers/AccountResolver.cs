using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Services;

namespace HoneyDo.Web.GraphQL.Resolvers
{
    public class AccountResolver
    {
        private readonly AccountService _accountService;

        public AccountResolver(AccountService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        /// <summary>Get current user's account.</summary>
        public async Task<Account> Account() => await _accountService.Get();

        /// <summary>Search for other accounts.</summary>
        public async Task<List<Account>> Accounts(string term) => await _accountService.Get(term);

        // TODO register
        // TODO add login
    }
}
