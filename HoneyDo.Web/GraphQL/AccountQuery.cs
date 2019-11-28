using System;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;

namespace HoneyDo.Web.GraphQL
{
    public class AccountQuery
    {
        private readonly IRepository<Account> _accountRepo;
        private readonly IAccountAccessor _accountAccessor;

        public AccountQuery(IRepository<Account> accountRepo,
            IAccountAccessor accountAccessor)
        {
            _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
            _accountAccessor = accountAccessor ?? throw new ArgumentNullException(nameof(accountAccessor));
        }

        public async Task<Account> GetAccount()
        {
            var account = await _accountAccessor.GetAccount();
            return account;
        }
    }
}
