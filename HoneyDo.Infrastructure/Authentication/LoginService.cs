using System;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;

namespace HoneyDo.Infrastructure.Authentication
{
    public class LoginService
    {
        private readonly string _pathToCredentialsJson;
        private readonly ILogger _logger;
        private bool _hasInitialized = false;

        public LoginService(string pathToCredentialsJson, ILogger<LoginService> logger)
        {
            _pathToCredentialsJson = pathToCredentialsJson;
            _logger = logger;
        }

        private void InitializeIfNeeded()
        {
                if (!_hasInitialized)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(_pathToCredentialsJson)
                    });
                    _hasInitialized = true;
                }
        }

        public async Task<string> BuildJwt(string firebaseToken)
        {
            InitializeIfNeeded();
            FirebaseToken decodedToken;
            try
            {
                decodedToken = await FirebaseAuth.DefaultInstance
                    .VerifyIdTokenAsync(firebaseToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "BuildJwt Error");
                return string.Empty;
            }
            string uid = decodedToken.Uid;
            return uid;
        }
    }
}