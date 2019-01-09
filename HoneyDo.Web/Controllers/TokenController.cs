using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Infrastructure.Authentication;
using HoneyDo.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.Annotations;

namespace HoneyDo.Web.Controllers
{
    [ApiController]
    [Route("api/token")]
    [Produces("application/json")]
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

        /// <remarks>
        /// Expects the following header:
        /// 
        ///     {
        ///         "Id-Token": "{token from firebase}
        ///     }
        /// 
        /// </remarks>
        [HttpGet("login")]
        [SwaggerOperation(Summary = "Create JWT for registered user.", OperationId = "CreateToken")]
        [SwaggerResponse(200, "Returns valid JWT.", typeof(TokenModel))]
        [SwaggerResponse(400, "Failed to find Id-Token header, validate Id-Token or find account.")]
        public async Task<ActionResult<TokenModel>> Login()
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

        /// <remarks>
        /// Expects the following header:
        /// 
        ///     {
        ///         "Id-Token": "{token from firebase}
        ///     }
        /// 
        /// </remarks>
        [HttpGet("register")]
        [SwaggerOperation(Summary = "Registers account and creates JWT.", OperationId = "Register")]
        [SwaggerResponse(200, "Returns valid JWT.", typeof(TokenModel))]
        [SwaggerResponse(400, "Failed to find Id-Token header or validate Id-Token.")]
        public async Task<ActionResult<TokenModel>> Register()
        {
            var idToken = GetIdToken();
            if (string.IsNullOrEmpty(idToken))
            {
                return BadRequest();
            }

            var account = await _loginService.RegisterViaToken(idToken);
            if (account == null)
            {
                return await Login();
            }
            var jwt = _loginService.GenerateToken(account);
            return Ok(new TokenModel(jwt));
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/login/test-token
        ///     {
        ///        "provider": "Google",
        ///        "providerId": "{valid provider id}",
        ///        "providerKey": "{valid provider key, not always needed}"
        ///     }
        ///
        /// </remarks>
        [HttpPost("test-token")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create JWT for provided login information.",
            OperationId = "CreateTestToken",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(200, "Returns valid JWT.", typeof(TokenModel))]
        [SwaggerResponse(400, "Login for provided provider and id not found.")]
        [SwaggerResponse(404, "Does not exist in current environment.")]
        public async Task<ActionResult<TokenModel>> TestToken(
            [FromBody, Required]
            [SwaggerParameter("Values to use when searching for login.")]
            LoginModel model)
        {
            if (!_environment.IsDevelopment())
            {
                return NotFound();
            }

            var account = await _loginService.FindAccountViaLoginModel(model);
            if (account == null)
            {
                return BadRequest();
            }
            var jwt = _loginService.GenerateToken(account);
            return Ok(new TokenModel(jwt));
        }
    }
}
