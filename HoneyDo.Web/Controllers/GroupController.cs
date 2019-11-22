using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneyDo.Domain.Entities;
using HoneyDo.Web.Models;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Groups;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using HoneyDo.Domain.Specifications.Accounts;

namespace HoneyDo.Web.Controllers
{
    [Route("api/groups"), Authorize, ApiController]
    [SwaggerTag("Create, read, update & delete groups.")]
    [Produces("application/json")]
    public class GroupController : Controller
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IAccountAccessor _accountAccessor;

        public GroupController(IRepository<Group> groupRepository, IAccountAccessor accountAccessor, IRepository<Account> accountRepository)
        {
            _groupRepository = groupRepository;
            _accountAccessor = accountAccessor;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all the groups that the user has access to.", OperationId = "GetGroups")]
        [SwaggerResponse(200, "All groups for the user.", typeof(List<Group>))]
        public async Task<ActionResult<List<Group>>> GetGroups()
        {
            var account = await _accountAccessor.GetAccount();
            var groups = await _groupRepository.Query(new GroupsForUser(account));
            return Ok(groups);
        }
    }
}
