using FirebaseAdmin;
using FirebaseAdmin.Auth;
using HoneyDo.Domain.Entities;
using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HoneyDo.Infrastructure.Providers
{
    public class GoogleProvider : IProvider
    {
        private readonly GoogleOptions _options;
        private readonly ILogger _logger;
        public string Provider => "GOOGLE";

        public GoogleProvider(IOptions<GoogleOptions> options,
            ILogger<GoogleProvider> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        private void InitializeIfNeeded()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var test = new AppOptions();
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(_options.FirebaseJson)
                });
            }
        }

        public async Task<string> GetProviderKey(string accessToken)
        {
            FirebaseToken token = await DecodeToken(accessToken);
            if (token == null)
            {
                return null;
            }
            string providerId = token.Uid;
            return providerId;
        }

        private async Task<FirebaseToken> DecodeToken(string firebaseToken)
        {
            InitializeIfNeeded();
            FirebaseToken decodedToken;
            try
            {
                decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(firebaseToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Verify Firebase Id Token Failed");
                return null;
            }
            return decodedToken;
        }
    }
}