using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Models;
using HoneyDo.Domain.Specifications.Accounts;
using HoneyDo.Domain.Specifications.Groups;

namespace HoneyDo.Domain.Services
{
    public class GroupService
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<GroupAccount> _groupAccountRepository;
        private readonly IAccountAccessor _accountAccessor;

        public GroupService(IRepository<Group> groupRepository,
            IAccountAccessor accountAccessor,
            IRepository<Account> accountRepository,
            IRepository<GroupAccount> groupAccountRepository)
        {
            _groupRepository = groupRepository;
            _accountAccessor = accountAccessor;
            _accountRepository = accountRepository;
            _groupAccountRepository = groupAccountRepository;
        }

        // TODO add authorization to all methods

        public async Task<List<Group>> Get() =>
             await _groupRepository.Query(new GroupsForAccount(await _accountAccessor.GetAccount()));

        public async Task<Group> Get(Guid id) =>
            await _groupRepository.Find(new GroupById(id));

        public async Task<Group> Create(IGroupForm model)
        {
            var account = await _accountAccessor.GetAccount();
            var group = new Group(model.Name, account);
            await _groupRepository.Add(group);
            return group;
        }

        public async Task<bool> Delete(Guid id)
        {
            var group = await Get(id);
            if (group == null)
            {
                return false;
            }

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
            {
                return false;
            }

            await _groupRepository.Remove(group);
            return true;
        }

        public async Task<Group> Update(Guid id, IGroupForm model)
        {
            var group = await _groupRepository.Find(new GroupById(id));
            if (group == null)
                return null;

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
                return null;

            group.UpdateName(model.Name);
            await _groupRepository.Update(group);
            return group;
        }

        public async Task<GroupAccount[]> AddAccounts(Guid id, Guid[] accountIds)
        {
            var group = await _groupRepository.Find(new GroupById(id), load: "_groupAccounts.Account");
            if (group == null)
                return new GroupAccount[0];

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
                return new GroupAccount[0];

            var accounts = await Task.WhenAll(accountIds.Select(async (accountId) =>
                  await _accountRepository.Find(new AccountById(accountId))));

            if (accounts.Any(acct => acct == null))
                return new GroupAccount[0]; // bad accountId given

            var existingAccounts = group.Accounts.Select(acct => acct.Id);
            if (accounts.Any(acct => existingAccounts.Contains(acct.Id)))
                return new GroupAccount[0]; // duplicate entries

            var groupAccounts = group.AddAccounts(accounts);
            await _groupRepository.Update(group);
            return groupAccounts;
        }

        public async Task<bool> RemoveAccounts(Guid id, Guid[] accountIds)
        {
            var group = await _groupRepository.Find(new GroupById(id), load: "_groupAccounts.Account");
            if (group == null)
                return false;

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
                return false;

            var existingAccounts = group.Accounts.Select(acct => acct.Id);
            if (accountIds.Any(accountId => !existingAccounts.Contains(accountId)))
                return false;

            group.RemoveAccounts(accountIds);
            await _groupRepository.Update(group);
            return true;
        }
    }
}
