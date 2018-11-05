using System;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Infrastructure.Authentication;
using HoneyDo.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HoneyDo.Web.Controllers
{
    [Route("api/token")]
    public class TokenController : Controller
    {
        private readonly LoginService _loginService;
        private readonly IHostingEnvironment _environment;

        public TokenController(LoginService loginService,
            IHostingEnvironment environment)
        {
            _loginService = loginService;
            _environment = environment;
        }

        private string GetIdToken()
        {

            if (!Request.Headers.TryGetValue("Id-Token", out StringValues idTokens) || idTokens.Count > 1)
            {
                return string.Empty;
            }
            var idToken = idTokens.First();
            return idToken;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            var idToken = GetIdToken();
            if (string.IsNullOrEmpty(idToken))
            {
                return BadRequest();
            }

            var account = await _loginService.FindAccountViaToken(idToken);
            if (account == null)
            {
                return BadRequest();
            }

            var jwt = _loginService.GenerateToken(account);
            return Ok(new TokenModel(jwt));
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            var idToken = GetIdToken();
            if (string.IsNullOrEmpty(idToken))
            {
                return BadRequest();
            }

            var account = await _loginService.RegisterViaToken(idToken);
            if (account == null)
            {
                return BadRequest(new TokenModel(string.Empty, "exists"));
            }
            var jwt = _loginService.GenerateToken(account);
            return Ok(new TokenModel(jwt));
        }

        [HttpPost("test-token"), AllowAnonymous]
        public IActionResult TestToken([FromBody] LoginModel model)
        {
            if (!_environment.IsDevelopment())
            {
                return NotFound();
            }

            var account = _loginService.FindAccountViaLoginModel(model);
            if (account == null)
            {
                return BadRequest();
            }
            var jwt = _loginService.GenerateToken(account);
            return Ok(new TokenModel(jwt));
        }
    }
}