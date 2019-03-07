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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HoneyDo.Web.Controllers
{
    [Route("api/account"), ApiController]
    [SwaggerTag("Register, read, update, and delete Accounts.")]
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _userManager;
        private readonly IEnumerable<IProvider> _providers;

        public AccountController(UserManager<Account> userManager,
            IEnumerable<IProvider> providers)
        {
            _userManager = userManager;
            _providers = providers;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Registers a new account.", OperationId = "RegisterAccount")]
        [SwaggerResponse(201, "The account was created.", typeof(Account))]
        [SwaggerResponse(400, "Unable to create account.")]
        public async Task<ActionResult> Register(
            [FromBody, Required]
            [SwaggerParameter("Account and login values.")]
                RegisterModel model)
        {
            Account account = new Account(model.Name, model.UserName, model.Picture);
            IdentityResult result = await _userManager.CreateAsync(account);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var loginResult = await AddLogin(account, model);
            if (!loginResult.Succeeded)
            {
                await _userManager.DeleteAsync(account);
                return BadRequest(loginResult.Errors);
            }

            return Created("api/account/", account);
        }

        [Authorize, HttpPost("login")]
        [SwaggerOperation(Summary = "Adds a new login to the account.", OperationId = "AddLogin")]
        [SwaggerResponse(204, "The login was created.")]
        [SwaggerResponse(400, "Unable to create login.")]
        public async Task<ActionResult> AddLogin([FromBody, Required] LoginModel model)
        {
            Account account = await _userManager.GetUserAsync(User);
            var result = await AddLogin(account, model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        private async Task<IdentityResult> AddLogin(Account account, LoginModel model)
        {
            // get provider
            IProvider provider = _providers.FirstOrDefault(p =>
                p.Provider.Equals(model.Provider, StringComparison.InvariantCultureIgnoreCase));
            if (provider == null)
            {
                IdentityError error = new IdentityError
                {
                    Code = "InvalidProvider",
                    Description = $@"Invalid or unsupported provider ""${model.Provider}"""
                };
                return IdentityResult.Failed(error);
            }

            // get provider key
            string providerKey = await provider.GetProviderKey(model.AccessToken);
            if (string.IsNullOrWhiteSpace(providerKey))
            {
                IdentityError error = new IdentityError
                {
                    Code = "InvalidAccessToken",
                    Description = $@"Unable to validate token with provider ""${provider.Provider}"""
                };
                return IdentityResult.Failed(error);
            }

            // add login
            var login = new UserLoginInfo(provider.Provider, providerKey, string.Empty);
            var result = await _userManager.AddLoginAsync(account, login);
            return result;
        }
    }
}