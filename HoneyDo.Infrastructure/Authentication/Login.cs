using System;
using HoneyDo.Domain.Entities;
using HoneyDo.Infrastructure.Enumerations;

namespace HoneyDo.Infrastructure.Authentication
{
    public class Login
    {
        public Guid Id { get; private set; }
        public Guid AccountId { get; private set; }
        public string Provider { get; private set; }
        public string ProviderId { get; private set; }
        public string ProviderKey { get; private set; }

        [Obsolete("system constructor")]
        protected Login() { }
        internal Login(Account account, Providers provider, string providerId, string providerKey)
        {
            Id = Guid.NewGuid();
            AccountId = account.Id;
            Provider = provider.ToString();
            ProviderId = providerId;
            ProviderKey = providerKey;
        }
    }
}
