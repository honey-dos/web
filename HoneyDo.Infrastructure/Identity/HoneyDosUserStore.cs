using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Accounts;
using HoneyDo.Infrastructure.Authentication;
using HoneyDo.Infrastructure.Specifications;
using Microsoft.AspNetCore.Identity;

namespace HoneyDo.Infrastructure.Identity
{
    public class HoneyDosUserStore : IUserStore<Account>, IUserLoginStore<Account>
    {
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<Login> _loginRepo;
        public HoneyDosUserStore(IRepository<Account> accountRepo, IRepository<Login> loginRepo)
        {
            _accountRepo = accountRepo;
            _loginRepo = loginRepo;
        }

        public async Task AddLoginAsync(Account user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            Login HoneyDoLogin = new Login(user, login.LoginProvider, login.ProviderKey);
            await _loginRepo.Add(HoneyDoLogin);
        }

        public async Task<IdentityResult> CreateAsync(Account user, CancellationToken cancellationToken)
        {
            await _accountRepo.Add(user);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Account user, CancellationToken cancellationToken)
        {
            bool isDeleted = await _accountRepo.Remove(user);
            if (isDeleted)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.Id}" });
        }

        public void Dispose()
        {
        }

        public async Task<Account> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            Guid id;
            if (!Guid.TryParse(userId, out id))
            {
                return null;
            }

            Account account = await _accountRepo.Find(new AccountById(id));
            return account;
        }

        public async Task<Account> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            Login login = await _loginRepo.Find(new LoginByProvider(loginProvider, providerKey));
            if (login == null)
            {
                return null;
            }
            Account account = await _accountRepo.Find(new AccountById(login.AccountId));
            return account;
        }

        public async Task<Account> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _accountRepo.Find(new AccountByUserName(normalizedUserName));
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(Account user, CancellationToken cancellationToken)
        {
            List<Login> logins = await _loginRepo.Query(new LoginsForAccount(user));
            var userLoginInfos = logins.Select(l => new UserLoginInfo(l.Provider, l.ProviderKey, string.Empty));
            return userLoginInfos.ToList(); ;
        }

        public async Task<string> GetNormalizedUserNameAsync(Account user, CancellationToken cancellationToken)
        {
            return await Task.Run(() => user.NormalizedUserName);
        }

        public async Task<string> GetUserIdAsync(Account user, CancellationToken cancellationToken)
        {
            return await Task.Run(() => user.Id.ToString());
        }

        public async Task<string> GetUserNameAsync(Account user, CancellationToken cancellationToken)
        {
            return await Task.Run(() => user.UserName);
        }

        public async Task RemoveLoginAsync(Account user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            Login login = await _loginRepo.Find(new LoginByProvider(loginProvider, providerKey));
            if (login == null)
            {
                return;
            }
            await _loginRepo.Remove(login);
        }

        public async Task SetNormalizedUserNameAsync(Account user, string normalizedName, CancellationToken cancellationToken)
        {
            await Task.Run(() => user.UpdateNormalizedUserName(normalizedName));
        }

        public async Task SetUserNameAsync(Account user, string userName, CancellationToken cancellationToken)
        {
            await Task.Run(() => user.UpdateUserName(userName));
        }

        public async Task<IdentityResult> UpdateAsync(Account user, CancellationToken cancellationToken)
        {
            await _accountRepo.Update(user);

            return IdentityResult.Success;
        }
    }
}