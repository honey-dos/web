using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneyDo.Domain.Entities;
using HoneyDo.Web.Models;
using HoneyDo.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using HoneyDo.Domain.Services;

namespace HoneyDo.Web.Controllers
{
    [Route("api/todos"), Authorize, ApiController]
    [SwaggerTag("Create, read, update & delete todos.")]
    [Produces("application/json")]
    public class TodoController : Controller
    {
        private readonly TodoService _todoService;

        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all the todos that the user has access to.", OperationId = "GetTodos")]
        [SwaggerResponse(200, "All todos for the user.", typeof(List<Todo>))]
        public async Task<ActionResult<Todo[]>> GetTodos() => Ok(await _todoService.Get());

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a specific todo.", OperationId = "GetTodo")]
        [SwaggerResponse(200, "Returns the specified todo.", typeof(Todo))]
        [SwaggerResponse(400, "Todo not found with the specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> GetTodo([SwaggerParameter("Id of the todo.")]Guid id) =>
            (await _todoService.Get(id)).ForRestApi();

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/todos
        ///     {
        ///        "name": "Some name",
        ///        "dueDate": "2018-11-13T23:53:09.651Z",
        ///        "groupId": "c20e5e2b-42d0-4164-8d2f-d2b75095bf64",
        ///        "assigneeId": "c20e5e2b-42d0-4164-8d2f-d2b75095bf64"
        ///     }
        ///
        /// </remarks> 
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Todo.",
            OperationId = "CreateTodo",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(201, "The todo was created.", typeof(Todo))]
        public async Task<ActionResult<Todo>> CreateTodo(
            [FromBody, Required]
            [SwaggerParameter("Todo values")]
                TodoCreateForm model) =>
            (await _todoService.Create(model)).ForRestApi();

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a specific todo.", OperationId = "DeleteTodo")]
        [SwaggerResponse(204, "Todo was successfully deleted.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult> DeleteTodo(
            [SwaggerParameter("Id of todo to be deleted.")] Guid id) =>
            (await _todoService.Delete(id)).ForRestApi();

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/todos/{id}
        ///     {
        ///        "name": "Some name",
        ///        "dueDate": "2018-11-13T23:53:09.651Z",
        ///        "groupId": "c20e5e2b-42d0-4164-8d2f-d2b75095bf64",
        ///        "assigneeId": "c20e5e2b-42d0-4164-8d2f-d2b75095bf64",
        ///        "isComplete": false,
        ///        "removeGroup": false,
        ///        "removeDueDate": false,
        ///        "removeAssignee": false
        ///     }
        ///
        /// </remarks> 
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates a specific todo.",
            OperationId = "UpdateTodo",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(200, "Returns successfully updated Todo.", typeof(Todo))]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> UpdateTodo(
            [SwaggerParameter("Id of todo to be updated.")] Guid id,
            [FromBody, Required]
            [SwaggerParameter("Todo values")]
                TodoUpdateForm model) =>
            (await _todoService.Update(id, model)).ForRestApi();

        [HttpPut("{id}/complete")]
        [SwaggerOperation(Summary = "Completes a specific todo.", OperationId = "CompleteTodo")]
        [SwaggerResponse(200, "Returns sucessfully completed Todo.", typeof(Todo))]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> Complete(
            [SwaggerParameter("Id of todo to be completed.")] Guid id) =>
            await UpdateTodo(id, new TodoUpdateForm { IsComplete = true });

        [HttpDelete("{id}/complete")]
        [SwaggerOperation(Summary = "Uncompletes a specific todo.", OperationId = "UncompleteTodo")]
        [SwaggerResponse(200, "Returns sucessfully uncompleted Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> Uncomplete(
            [SwaggerParameter("Id of todo to be uncompleted.")] Guid id) =>
            await UpdateTodo(id, new TodoUpdateForm { IsComplete = false });

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/todos/{id}
        ///     "2018-11-13T23:53:09.651Z"
        ///
        /// </remarks> 
        [HttpPut("{id}/due")]
        [SwaggerOperation(Summary = "Adds due date to a specific todo.",
            OperationId = "AddDueDate",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(200, "Returns sucessfully updated Todo.", typeof(Todo))]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> AddDueDate(
            [SwaggerParameter("Id of todo to be updated.")] Guid id,
            [FromBody, Required]
            [SwaggerParameter("Due date value to add to todo.")]
                DateTime dueDate) =>
            await UpdateTodo(id, new TodoUpdateForm { DueDate = dueDate });

        [HttpDelete("{id}/due")]
        [SwaggerOperation(Summary = "Removes due date a specific todo.", OperationId = "RemoveDueDate")]
        [SwaggerResponse(200, "Returns sucessfully updated Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> RemoveDueDate(
            [SwaggerParameter("Id of todo to be updated.")] Guid id) =>
            await UpdateTodo(id, new TodoUpdateForm { DueDate = null });

        [HttpPut("{id}/assign/{accountId}")]
        [SwaggerOperation(Summary = "Assign account to a specific todo.", OperationId = "AssignTodo")]
        [SwaggerResponse(200, "Returns sucessfully updated Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> Assign(
                [SwaggerParameter("Id of todo to be updated.")] Guid id,
                [SwaggerParameter("Id of account to assign the todo to")] Guid accountId) =>
            await UpdateTodo(id, new TodoUpdateForm { AssigneeId = accountId });

        [HttpDelete("{id}/assign")]
        [SwaggerOperation(Summary = "Unassign a specific todo.", OperationId = "UnassignTodo")]
        [SwaggerResponse(200, "Returns sucessfully updated Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> Unassign(
                [SwaggerParameter("Id of todo to be updated.")] Guid id) =>
            await UpdateTodo(id, new TodoUpdateForm { RemoveAssignee = true });

        [HttpPut("{id}/group/{groupId}")]
        [SwaggerOperation(Summary = "Change a specific todo's group.", OperationId = "ChangeGroup")]
        [SwaggerResponse(200, "Returns sucessfully updated Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> ChangeGroup(
                [SwaggerParameter("Id of todo to be updated.")] Guid id,
                [SwaggerParameter("Id of todo to be updated.")] Guid groupId) =>
            await UpdateTodo(id, new TodoUpdateForm { GroupId = groupId });

        [HttpDelete("{id}/group")]
        [SwaggerOperation(Summary = "Remove a specific todo from a group.", OperationId = "RemoveGroup")]
        [SwaggerResponse(200, "Returns sucessfully updated Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> RemoveGroup(
                [SwaggerParameter("Id of todo to be updated.")] Guid id) =>
            await UpdateTodo(id, new TodoUpdateForm { RemoveGroup = true });
    }
}
