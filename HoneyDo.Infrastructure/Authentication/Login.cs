using System;
using HoneyDo.Domain.Entities;

namespace HoneyDo.Infrastructure.Authentication
{
    public class Login
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public string Provider { get; private set; }
        public string ProviderKey { get; private set; }

        [Obsolete("system constructor")]
        protected Login() { }
        internal Login(Account account, string provider, string providerKey)
        {
            Id = Guid.NewGuid();
            AccountId = account.Id;
            Provider = provider.ToString();
            ProviderKey = providerKey;
        }
    }
}