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
using HoneyDo.Domain.Specifications.GroupAccounts;

namespace HoneyDo.Web.Controllers
{
    [Route("api/groups"), Authorize, ApiController]
    [SwaggerTag("Create, read, update & delete groups.")]
    [Produces("application/json")]
    public class GroupController : Controller
    {
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<GroupAccount> _groupAccountRepository;
        private readonly IAccountAccessor _accountAccessor;

        public GroupController(IRepository<Group> groupRepository,
            IAccountAccessor accountAccessor,
            IRepository<Account> accountRepository,
            IRepository<GroupAccount> groupAccountRepository)
        {
            _groupRepository = groupRepository;
            _accountAccessor = accountAccessor;
            _accountRepository = accountRepository;
            _groupAccountRepository = groupAccountRepository;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all the groups that the user has access to.", OperationId = "GetGroups")]
        [SwaggerResponse(200, "All groups for the user.", typeof(List<Group>))]
        [SwaggerResponse(400, "Load parameter no valid.")]
        public async Task<ActionResult<List<Group>>> GetGroups(
            [SwaggerParameter("Additional entities to fetch ('Tasks|Accounts').")]string load = "")
        {
            string loadActual;
            switch (load.ToLower())
            {
                case "tasks":
                    loadActual = "_tasks";
                    break;
                case "accounts":
                    loadActual = "_groupAccounts.Account";
                    break;
                case "":
                    loadActual = "";
                    break;
                default:
                    return BadRequest();
            }
            var account = await _accountAccessor.GetAccount();
            var groups = await _groupRepository.Query(new GroupsForUser(account), load: loadActual);
            return Ok(groups);
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/groups
        ///     {
        ///        "name": "Some name"
        ///     }
        ///
        /// </remarks> 
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Group.",
            OperationId = "CreateGroup",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(201, "The group was created.", typeof(Group))]
        public async Task<ActionResult<Group>> CreateGroup(
            [FromBody, Required]
            [SwaggerParameter("Group values, optional: dueDate")]
                GroupCreateFormModel model)
        {
            var account = await _accountAccessor.GetAccount();
            var group = new Group(model.Name, account);
            await _groupRepository.Add(group);
            return Created($"api/groups/{group.Id}", group);
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/groups/{id}
        ///     {
        ///        "name": "Some name"
        ///     }
        ///
        /// </remarks> 
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates a specific group.",
            OperationId = "UpdateGroup",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(200, "Returns successfully updated Group.", typeof(Group))]
        [SwaggerResponse(400, "No group found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific group.")]
        public async Task<ActionResult<Group>> UpdateGroup(
            [SwaggerParameter("Id of group to be updated.")] Guid id,
            [FromBody, Required]
            [SwaggerParameter("Group values, optional: dueDate")]
                GroupCreateFormModel model)
        {
            var group = await _groupRepository.Find(new GroupById(id));
            if (group == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
            {
                return Unauthorized();
            }

            group.UpdateName(model.Name);
            await _groupRepository.Update(group);
            return Ok(group);
        }

        [HttpPost("{id}/add/{accountId}")]
        [SwaggerOperation(Summary = "Add account to a specific group.", OperationId = "CreateGroupAccount")]
        [SwaggerResponse(200, "Returns sucessfully created GroupAccount.")]
        [SwaggerResponse(400, "No group or account found with specified id or account already belongs to group.")]
        [SwaggerResponse(403, "Don't have access to specific group.")]
        public async Task<ActionResult<GroupAccount>> CreateGroupAccount(
                [SwaggerParameter("Id of group to add account to.")] Guid id,
                [SwaggerParameter("Id of account to be added to group.")] Guid accountId)
        {
            var group = await _groupRepository.Find(new GroupById(id));
            if (group == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
            {
                return Unauthorized();
            }

            account = await _accountRepository.Find(new AccountById(accountId));
            if (account == null)
            {
                return BadRequest();
            }

            var groupAccount = await _groupAccountRepository.Find(new GroupAccountByIds(id, accountId));
            if (groupAccount != null)
            {
                return BadRequest();
            }

            groupAccount = new GroupAccount(group, account);
            await _groupAccountRepository.Add(groupAccount);
            return Ok(groupAccount);
        }

        [HttpDelete("{id}/remove/{accountId}")]
        [SwaggerOperation(Summary = "Remove account from a specific group.", OperationId = "RemoveGroupAccount")]
        [SwaggerResponse(204, "GroupAccount was successfully deleted.")]
        [SwaggerResponse(400, "No group or account found with specified id or account does not belong to group.")]
        [SwaggerResponse(403, "Don't have access to specific group.")]
        public async Task<ActionResult> RemoveGroupAccount(
                [SwaggerParameter("Id of group to remove account from.")] Guid id,
                [SwaggerParameter("Id of account to be removed.")] Guid accountId)
        {
            var group = await _groupRepository.Find(new GroupById(id));
            if (group == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (group.CreatorId != account.Id)
            {
                return Unauthorized();
            }

            account = await _accountRepository.Find(new AccountById(accountId));
            if (account == null)
            {
                return BadRequest();
            }

            var groupAccount = await _groupAccountRepository.Find(new GroupAccountByIds(id, accountId));
            if (groupAccount == null)
            {
                return BadRequest();
            }

            await _groupAccountRepository.Remove(groupAccount);
            return NoContent();
        }

    }
}
