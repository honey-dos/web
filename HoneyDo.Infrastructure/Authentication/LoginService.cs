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

        private async Task<Login> FindLogin(Providers provider, string providerId) => await _loginRepo.Find(new FindLogin(provider, providerId));

        private void InitializeIfNeeded()
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var test = new AppOptions();
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(_loginOptions.FirebaseJson)
                });
            }
        }

        public async Task<Account> RegisterViaToken(string firebaseToken)
        {
            FirebaseToken token = await DecodeToken(firebaseToken);
            string providerId = token.Uid;

            Login existingLogin = await FindLogin(Providers.Google, providerId);
            if (existingLogin != null)
            {
                return null;
            }
            string name = token.Claims.FirstOrDefault(claim => claim.Key == "name").Value.ToString();
            string picture = token.Claims.FirstOrDefault(claim => claim.Key == "picture").Value.ToString();

            Account account = new Account(name);
            account.UpdatePicture(picture);
            await _accountRepo.Add(account);

            Login login = new Login(account, Providers.Google, providerId, string.Empty);
            await _loginRepo.Add(login);

            return account;
        }

        public async Task<Account> FindAccountViaToken(string firebaseToken)
        {
            FirebaseToken token = await DecodeToken(firebaseToken);
            string providerId = token.Uid;
            Login login = await FindLogin(Providers.Google, providerId);
            if (login == null)
            {
                return null;
            }

            Account account = await _accountRepo.Find(new FindAccount(login.AccountId));
            return account;
        }

        public async Task<Account> FindAccountViaLoginModel(LoginModel model)
        {
            Login login = await FindLogin(Providers.Google, model.ProviderId);
            if (login == null)
            {
                return null;
            }

            Account account = await _accountRepo.Find(new FindAccount(login.AccountId));
            return account;
        }

        public string GenerateToken(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Name),
                new Claim(JwtRegisteredClaimNames.Jti, account.Id.ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_loginOptions.Key));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(_loginOptions.Issuer,
            _loginOptions.Issuer,
            claims,
            expires: DateTime.Now.AddMilliseconds(_loginOptions.MillisecondsUntilExpiration),
            signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
