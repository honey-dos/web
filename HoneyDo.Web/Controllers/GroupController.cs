using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneyDo.Domain.Entities;
using HoneyDo.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using HoneyDo.Domain.Services;

namespace HoneyDo.Web.Controllers
{
    [Route("api/groups"), Authorize, ApiController]
    [SwaggerTag("Create, read, update & delete groups.")]
    [Produces("application/json")]
    public class GroupController : Controller
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all the groups that the user has access to.", OperationId = "GetGroups")]
        [SwaggerResponse(200, "All groups for the user.", typeof(List<Group>))]
        [SwaggerResponse(400, "Load parameter no valid.")]
        public async Task<ActionResult<List<Group>>> GetGroups() => Ok(await _groupService.Get());

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a specific group.", OperationId = "GetGroup")]
        [SwaggerResponse(200, "Returns the specified group.", typeof(Group))]
        [SwaggerResponse(400, "Group not found with the specified id.")]
        [SwaggerResponse(403, "Don't have access to specific group.")]
        public async Task<ActionResult<Group>> GetGroup([SwaggerParameter("Id of the group.")]Guid id)
        {
            var group = await _groupService.Get(id);
            if (group == null)
                return BadRequest();

            return Ok(group);
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
                GroupCreateForm model)
        {
            var group = await _groupService.Create(model);
            return Created($"api/groups/{group.Id}", group);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a specific group.", OperationId = "DeleteGroup")]
        [SwaggerResponse(204, "Group was successfully deleted.")]
        [SwaggerResponse(400, "No group found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific group.")]
        public async Task<ActionResult> DeleteGroup(
            [SwaggerParameter("Id of group to be deleted.")] Guid id)
        {
            var isDeleted = await _groupService.Delete(id);
            if (!isDeleted)
                return BadRequest();

            return NoContent();
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
            [SwaggerParameter("Group values.")]
                GroupCreateForm model)
        {
            var updatedGroup = await _groupService.Update(id, model);
            if (updatedGroup == null)
                return BadRequest();

            return Ok(updatedGroup);
        }

        [HttpPost("{id}/add/accounts")]
        [SwaggerOperation(Summary = "Add accounts to a specific group.", OperationId = "CreateGroupAccounts")]
        [SwaggerResponse(200, "Returns sucessfully created GroupAccounts.")]
        [SwaggerResponse(400, "No group or account(s) found with specified id or account already belongs to group.")]
        [SwaggerResponse(403, "Don't have access to specific group.")]
        public async Task<ActionResult<GroupAccount[]>> CreateGroupAccounts(
                [SwaggerParameter("Id of group to add account to.")] Guid id,
                [FromBody, Required]
                [SwaggerParameter("Ids of accounts to be added to group.")]
                    Guid[] accountIds)
        {
            if (!accountIds.Any())
                return BadRequest();

            var groupAccounts = await _groupService.AddAccounts(id, accountIds);
            if (!groupAccounts.Any())
                return BadRequest();

            return Ok(groupAccounts);
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
            var groupAccounts = await _groupService.AddAccounts(id, new Guid[] { accountId });
            if (!groupAccounts.Any())
                return BadRequest();

            return Ok(groupAccounts[0]);
        }

        [HttpDelete("{id}/remove/accounts")]
        [SwaggerOperation(Summary = "Remove accounts from a specific group.", OperationId = "RemoveGroupAccounts")]
        [SwaggerResponse(204, "GroupAccounts were successfully deleted.")]
        [SwaggerResponse(400, "No group or account(s) found with specified id or account does not belong to group.")]
        [SwaggerResponse(403, "Don't have access to specific group.")]
        public async Task<ActionResult> RemoveGroupAccounts(
                [SwaggerParameter("Id of group to remove account from.")] Guid id,
                [FromBody, Required]
                [SwaggerParameter("Ids of accounts to be added to group.")]
                    Guid[] accountIds)
        {
            if (!accountIds.Any())
                return BadRequest();

            var areRemoved = await _groupService.RemoveAccounts(id, accountIds);
            if (!areRemoved)
                return BadRequest();

            return NoContent();
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
            var isRemoved = await _groupService.RemoveAccounts(id, new Guid[] { accountId });
            if (!isRemoved)
                return BadRequest();

            return NoContent();
        }

    }
}
