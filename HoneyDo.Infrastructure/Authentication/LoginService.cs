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
using HoneyDo.Infrastructure.Enumerations;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Infrastructure.Specifications;

namespace HoneyDo.Infrastructure.Authentication
{
    public class LoginService
    {
        private readonly LoginOptions _loginOptions;
        private readonly IRepository<Login> _loginRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly ILogger _logger;

        public LoginService(IOptions<LoginOptions> loginOptions,
            IRepository<Login> loginRepo,
            IRepository<Account> accountRepo,
            ILogger<LoginService> logger)
        {
            _loginOptions = loginOptions.Value;
            _loginRepo = loginRepo;
            _accountRepo = accountRepo;
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
            catch (Exception exception)
            {
                _logger.LogError(exception, "Verify Firebase Id Token Failed");
                return null;
            }
            return decodedToken;
        }

        private Login FindLogin(Providers provider, string providerId) => _loginRepo.Find(new FindLogin(provider, providerId));

        private void InitializeIfNeeded()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(_loginOptions.PathToCredentialsJson)
                });
            }
        }

        public async Task<Account> Register(string firebaseToken)
        {
            var token = await DecodeToken(firebaseToken);
            var providerId = token.Uid;
            var name = token.Claims.FirstOrDefault(claim => claim.Key == "name");
            var picture = token.Claims.FirstOrDefault(claim => claim.Key == "picture");

            var existingLogin = FindLogin(Providers.Google, providerId);
            if (existingLogin != null)
            {
                return null;
            }

            var account = new Account(name.Value.ToString());
            account.UpdatePicture(picture.Value.ToString());
            _accountRepo.Add(account);

            var login = new Login(account, Providers.Google, providerId, string.Empty);
            _loginRepo.Add(login);

            return account;
        }

        public async Task<Account> FindAccountForToken(string firebaseToken)
        {
            var token = await DecodeToken(firebaseToken);
            var providerId = token.Uid;
            var login = FindLogin(Providers.Google, providerId);
            if (login == null)
            {
                return null;
            }

            var account = _accountRepo.Find(new FindAccount(login.AccountId));
            return account;
        }

        public string GenerateToken(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

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