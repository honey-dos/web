using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Logging;
using HoneyDo.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Linq;
using Avocado.Infrastructure.Authentication;
using HoneyDo.Infrastructure.Enumerations;

namespace HoneyDo.Infrastructure.Authentication
{
    public class LoginService
    {
        private readonly LoginOptions _loginOptions;
        private readonly ILogger _logger;
        private bool _hasInitialized = false;

        public LoginService(IOptions<LoginOptions> loginOptions, ILogger<LoginService> logger)
        {
            _loginOptions = loginOptions.Value;
            _logger = logger;
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
            catch (Exception e)
            {
                _logger.LogError(e, "Verify Firebase Id Token Failed");
                return null;
            }
            return decodedToken;
        }

        private void InitializeIfNeeded()
        {
                if (!_hasInitialized)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(_loginOptions.PathToCredentialsJson)
                    });
                    _hasInitialized = true;
                }
        }

        public async Task<Account> Register(string firebaseToken)
        {
            var token = await DecodeToken(firebaseToken);
            var providerId = token.Uid;
            var name = token.Claims.FirstOrDefault(claim => claim.Key == "name");
            var picture = token.Claims.FirstOrDefault(claim => claim.Key == "picture");

            // TODO look for account first

            var account = new Account(name.Value.ToString());
            account.UpdatePicture(picture.Value.ToString());

            var login = new Login(account, Providers.Google, providerId, string.Empty);

            // TODO save account and login

            return account;
        }

        public async Task<Account> FindAccountForToken(string firebaseToken)
        {
            var token = await DecodeToken(firebaseToken);
            var providerId = token.Uid;
            return null;
        }

        public string GenerateToken(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Name),
                new Claim(JwtRegisteredClaimNames.Jti, account.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_loginOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(_loginOptions.Issuer,
            _loginOptions.Issuer,
            claims,
            expires: DateTime.Now.AddMilliseconds(_loginOptions.MillisecondsUntilExpiration),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}