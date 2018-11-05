using System;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HoneyDo.Web.Controllers
{
    [Route("api/token")]
    public class TokenController : Controller
    {
        private readonly LoginService _loginService;

        public TokenController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            if (!Request.Headers.TryGetValue("Id-Token", out StringValues idTokens) || idTokens.Count > 1)
            {
                return BadRequest();
            }
            var idToken = idTokens.First();

            var result = await _loginService.BuildJwt(idToken);
            return Ok(new { token = result });
        }
    }
}