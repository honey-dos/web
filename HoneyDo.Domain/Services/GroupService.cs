using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Models;
using HoneyDo.Domain.Specifications.Accounts;
using HoneyDo.Domain.Specifications.Groups;
using HoneyDo.Domain.Values.Errors;

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

        public async Task<IDomainResult<Group>> Get(Guid id)
        {
            var group = await _groupRepository.Find(new GroupById(id));
            if (group == null)
                return new NotFoundResult<Group>();

            return new SuccessResult<Group>(group);
        }

        public async Task<List<Group>> Get(Account account) =>
            (await _accountAccessor.GetAccount()).Id == account.Id
                ? await Get()
                : null;

        public async Task<IDomainResult<Group>> Create(IGroupForm input)
        {
            if (input == null)
                return new InvalidArgumentResult<Group>(nameof(input));

            var account = await _accountAccessor.GetAccount();
            var group = new Group(input.Name, account);
            await _groupRepository.Add(group);
            return new CreatedResult<Group>(group);
        }

        public async Task<IDomainResult> Delete(Guid id)
        {
            var result = await Get(id);
            if (result.HasError)
                return result as IDomainResult;

            var group = result.Value;
            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
                return new InsufficientPermissionsResult();

            await _groupRepository.Remove(group);
            return new DeletedResult();
        }

        public async Task<IDomainResult<Group>> Update(Guid id, IGroupForm model)
        {
            var result = await Get(id);
            if (result.HasError)
                return result;

            var group = result.Value;
            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
                return new InsufficientPermissionsResult<Group>();

            group.UpdateName(model.Name);
            await _groupRepository.Update(group);
            return new SuccessResult<Group>(group);
        }

        public async Task<IDomainResult<GroupAccount[]>> AddAccounts(Guid id, Guid[] accountIds)
        {
            var group = await _groupRepository.Find(new GroupById(id), load: "_groupAccounts.Account");
            if (group == null)
                return new NotFoundResult<GroupAccount[]>();

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
                return new InsufficientPermissionsResult<GroupAccount[]>();

            var accounts = await Task.WhenAll(accountIds.Select(async (accountId) =>
                  await _accountRepository.Find(new AccountById(accountId))));

            if (accounts.Any(acct => acct == null))
                return new InvalidArgumentResult<GroupAccount[]>(nameof(accountIds));

            var existingAccounts = group.Accounts.Select(acct => acct.Id);
            if (accounts.Any(acct => existingAccounts.Contains(acct.Id)))
                return new InvalidArgumentResult<GroupAccount[]>(nameof(accountIds));

            var groupAccounts = group.AddAccounts(accounts);
            await _groupRepository.Update(group);
            return new CreatedResult<GroupAccount[]>(groupAccounts);
        }

        public async Task<IDomainResult> RemoveAccounts(Guid id, Guid[] accountIds)
        {
            var group = await _groupRepository.Find(new GroupById(id), load: "_groupAccounts.Account");
            if (group == null)
                return new NotFoundResult();

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
                return new InsufficientPermissionsResult();

            var existingAccounts = group.Accounts.Select(acct => acct.Id);
            if (accountIds.Any(accountId => !existingAccounts.Contains(accountId)))
                return new InvalidArgumentResult(nameof(accountIds));

            group.RemoveAccounts(accountIds);
            await _groupRepository.Update(group);
            return new DeletedResult();
        }
    }
}
