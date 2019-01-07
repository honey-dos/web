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
    /// <summary>
    /// controller to handle token creation and registration
    /// </summary>
    [ApiController]
    [Route("api/token")]
    [Produces("application/json")]
    public class TokenController : Controller
    {
        private readonly LoginService _loginService;
        private readonly IHostingEnvironment _environment;

        /// <summary>
        /// controller constructor
        /// </summary>
        /// <param name="loginService">login service, handles token creation and registration</param>
        /// <param name="environment">hosting environment</param>
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

        /// <summary>
        /// Create token for registered user.
        /// </summary>
        /// <remarks>
        /// Expects the following header:
        /// 
        ///     {
        ///         "Id-Token": "{token from firebase}
        ///     }
        /// 
        /// </remarks>
        /// <returns> Newly created JWT</returns>
        /// <response code="200">Returns the newly created JWT</response>
        /// <response code="400">Failed to find Id-Token header, validate Id-Token or find account</response>
        [HttpGet("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Registers account and creates token.
        /// </summary>
        /// <remarks>
        /// Expects the following header:
        /// 
        ///     {
        ///         "Id-Token": "{token from firebase}
        ///     }
        /// 
        /// </remarks>
        /// <returns> Newly created JWT.</returns>
        /// <response code="200">Returns the newly created JWT.</response>
        /// <response code="400">Failed to find Id-Token header or validate Id-Token</response>
        [HttpGet("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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

        /// <summary>
        /// Creates a valid token for the provided login if valid.
        /// </summary>
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
        /// <param name="model"></param>
        /// <returns>Newly created JWT.</returns>
        /// <response code="201">Returns the newly created JWT.</response>
        /// <response code="400">Login for provided provider and id not found.</response>
        /// <response code="404">Does not exist in current environment.</response>
        [HttpPost("test-token")]
        [AllowAnonymous]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TokenModel>> TestToken([FromBody] LoginModel model)
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
