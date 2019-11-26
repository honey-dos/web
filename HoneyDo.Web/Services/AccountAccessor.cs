
using System;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Accounts;
using Microsoft.AspNetCore.Http;

namespace HoneyDo.Web.Services
{
    public class AccountAccessor : IAccountAccessor
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IRepository<Account> _accountRepo;
        private Account _account;

        public AccountAccessor(IHttpContextAccessor httpAccessor, IRepository<Account> accountRepo)
        {
            _httpAccessor = httpAccessor;
            _accountRepo = accountRepo;
        }

        public async Task<Account> GetAccount(string load = "")
        {
            if (_account != null)
            {
                return _account;
            }

            var user = _httpAccessor.HttpContext.User;
            if (user == null)
            {
                return null;
            };

            var claims = user.Claims;
            var idClaim = claims.FirstOrDefault(c => c.Type == "id");
            if (idClaim == null)
            {
                return null;
            };

            if (!Guid.TryParse(idClaim.Value, out Guid accountId))
            {
                return null;
            };

            _account = await _accountRepo.Find(new AccountById(accountId), load);
            return _account;
        }
    }
}
