using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.GroupAccounts;
using HoneyDo.Domain.Specifications.Accounts;

namespace HoneyDo.Domain.Services
{
    public class AccountService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IAccountAccessor _accountAccessor;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<GroupAccount> _groupAccountRepository;

        public AccountService(IAccountAccessor accountAccessor,
            IRepository<Account> accountRepository,
            IRepository<Group> groupRepository,
            IRepository<GroupAccount> groupAccountRepository)
        {
            _accountAccessor = accountAccessor;
            _accountRepository = accountRepository;
            _groupRepository = groupRepository;
            _groupAccountRepository = groupAccountRepository;
        }

        // TODO add authorization to all methods

        public async Task<Account> Get() => await _accountAccessor.GetAccount();

        public async Task<Account> Get(Guid id) => await _accountRepository.Find(new AccountById(id));

        public async Task<List<Account>> Get(string term) => 
            await _accountRepository.Query(new AccountsByTerm(term));

        public async Task<List<Account>> Get(Group group) =>
            (await _groupAccountRepository.Query(new GroupAccountForGroup(group), load: "Account"))
                .Select(ga => ga.Account)
                .ToList();
    }
}
