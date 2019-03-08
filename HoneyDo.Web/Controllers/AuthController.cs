using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Infrastructure.Authentication;
using HoneyDo.Infrastructure.Providers;
using HoneyDo.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace HoneyDo.Web.Controllers
{
    [Route("api/auth"), ApiController]
    [SwaggerTag("Generates JWTs and auth configuration.")]
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly UserManager<Account> _userManager;
        private readonly IEnumerable<IProvider> _providers;
        private readonly FirebaseConfig _firebaseConfig;
        private readonly IHostingEnvironment _environment;

        public AuthController(IJwtFactory jwtFactory,
            UserManager<Account> userManager,
            IEnumerable<IProvider> providers,
            IOptions<FirebaseConfig> firebaseConfig,
            IHostingEnvironment environment)
        {
            _jwtFactory = jwtFactory;
            _userManager = userManager;
            _providers = providers;
            _firebaseConfig = firebaseConfig.Value;
            _environment = environment;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Generates a new JWT.", OperationId = "GenerateToken")]
        [SwaggerResponse(200, "The token was created.", typeof(TokenModel))]
        [SwaggerResponse(400, "Unable to create token.")]
        public async Task<ActionResult> Token([FromBody, Required] LoginModel model)
        {
            // get provider
            IProvider provider = _providers.FirstOrDefault(p =>
                p.Provider.Equals(model.Provider, StringComparison.InvariantCultureIgnoreCase));
            if (provider == null)
            {
                return BadRequest();
            }

            // get provider key
            string providerKey = await provider.GetProviderKey(model.AccessToken);
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                return BadRequest();
            }

            // get account
            Account account = await _userManager.FindByLoginAsync(provider.Provider, providerKey);
            if (account == null)
            {
                return BadRequest();
            }

            // generate token
            string token = _jwtFactory.GenerateToken(account);
            return Ok(new TokenModel(token));
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/login/impersonate-token
        ///     {
        ///        "provider": "Google",
        ///        "providerKey": "{valid provider key}"
        ///     }
        ///
        /// </remarks>
        [HttpPost("impersonate-token")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create JWT for provided login information.",
            OperationId = "CreateImpersonateToken",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(200, "Returns valid JWT.", typeof(TokenModel))]
        [SwaggerResponse(400, "Login for provided provider and id not found.")]
        [SwaggerResponse(404, "Does not exist in current environment.")]
        public async Task<ActionResult<TokenModel>> ImpersonateToken(
            [FromBody, Required]
            [SwaggerParameter("Values to use when searching for login.")]
            ImpersonateModel model)
        {
            // only allowed in development
            if (!_environment.IsDevelopment())
            {
                return NotFound();
            }

            // find account
            var account = await _userManager.FindByLoginAsync(model.Provider, model.ProviderKey);
            if (account == null)
            {
                return BadRequest();
            }

            // generate token
            string token = _jwtFactory.GenerateToken(account);
            return Ok(new TokenModel(token));
        }

        [HttpGet("firebase-config")]
        [SwaggerOperation(Summary = "Gets firebase service account configuration.", OperationId = "FirebaseConfig")]
        [SwaggerResponse(200, "Firebase service account configuration.", typeof(FirebaseConfig))]
        public ActionResult FirebaseConfig()
        {
            return Ok(_firebaseConfig);
        }
    }
}