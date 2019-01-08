using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneyDo.Domain.Entities;
using HoneyDo.Web.Models;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Todos;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace HoneyDo.Web.Controllers
{
    [Route("api/todos"), Authorize, ApiController]
    [Produces("application/json")]
    [SwaggerResponse(401, "Not authenticated.")]
    public class TodoController : Controller
    {
        private readonly IRepository<Todo> _todoRepository;
        private readonly IAccountAccessor _accountAccessor;

        public TodoController(IRepository<Todo> todoRepository, IAccountAccessor accountAccessor)
        {
            _todoRepository = todoRepository;
            _accountAccessor = accountAccessor;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all the todos the user as access to.", OperationId = "GetTodos")]
        [SwaggerResponse(200, "All todos for the user.", typeof(List<Todo>))]
        public async Task<ActionResult<List<Todo>>> GetTodos()
        {
            var account = await _accountAccessor.GetAccount();
            var todos = await _todoRepository.Query(new TodosForUser(account));
            return Ok(todos);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a specific todo.", OperationId = "GetTodo")]
        [SwaggerResponse(200, "Returns the specified todo.", typeof(Todo))]
        [SwaggerResponse(400, "Todo not found with the specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> GetTodo([SwaggerParameter("Id of the todo.")]Guid id)
        {
            var todo = await _todoRepository.Find(new TodoById(id));
            if (todo == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (todo.OwnerId != account.Id)
            {
                return Unauthorized();
            }

            return Ok(todo);
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/todos
        ///     {
        ///        "name": "Some name",
        ///        "dueDate": "2018-11-13T23:53:09.651Z" (optional)
        ///     }
        ///
        /// </remarks> 
        [HttpPost]
        [SwaggerOperation(Summary = "Create a new Todo.",
            OperationId = "CreateTodo",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(201, "The todo was created.")]
        public async Task<ActionResult<Todo>> CreateTodo(
            [FromBody]
            [SwaggerParameter("Todo values, optional: dueDate")]
                TodoCreateFormModel model)
        {
            var account = await _accountAccessor.GetAccount();
            var todo = new Todo(model.Name, account);
            if (model.DueDate.HasValue)
            {
                todo.UpdateDueDate(model.DueDate);
            }
            await _todoRepository.Add(todo);
            return Created($"api/todos/{todo.Id}", todo);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a specific todo.", OperationId = "DeleteTodo")]
        [SwaggerResponse(204, "Todo was successfully deleted")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult> DeleteTodo(
            [SwaggerParameter("Id of todo to be deleted.")] Guid id)
        {
            var todo = await _todoRepository.Find(new TodoById(id));
            if (todo == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (todo.OwnerId != account.Id)
            {
                return Unauthorized();
            }

            await _todoRepository.Remove(todo);
            return NoContent();
        }

        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/todos/{id}
        ///     {
        ///        "name": "Some name",
        ///        "dueDate": "2018-11-13T23:53:09.651Z" (optional)
        ///     }
        ///
        /// </remarks> 
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates a specific todo.",
            OperationId = "UpdateTodo",
            Consumes = new[] { "application/json" })]
        [SwaggerResponse(200, "Returns successfully updatd Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> UpdateTodo(
            [SwaggerParameter("Id of todo to be updated.")] Guid id,
            [FromBody]
            [SwaggerParameter("Todo values, optional: dueDate")]
                TodoCreateFormModel model)
        {
            var todo = await _todoRepository.Find(new TodoById(id));

            if (todo == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (todo.OwnerId != account.Id)
            {
                return Unauthorized();
            }

            todo.UpdateName(model.Name);
            if (model.DueDate != todo.DueDate)
            {
                todo.UpdateDueDate(model.DueDate);
            }
            await _todoRepository.Update(todo);
            return Ok(todo);
        }

        [HttpPut("{id}/complete")]
        [SwaggerOperation(Summary = "Completes a specific todo.", OperationId = "CompleteTodo")]
        [SwaggerResponse(200, "Returns sucessfully completed Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> Complete(
            [SwaggerParameter("Id of todo to be completed.")] Guid id)
        {
            return await SetCompletion(id, true);
        }

        [HttpDelete("{id}/complete")]
        [SwaggerOperation(Summary = "Uncompletes a specific todo.", OperationId = "UncompleteTodo")]
        [SwaggerResponse(200, "Returns sucessfully uncompleted Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> Uncomplete(
            [SwaggerParameter("Id of todo to be uncompleted.")] Guid id)
        {
            return await SetCompletion(id, false);
        }

        private async Task<ActionResult<Todo>> SetCompletion(Guid id, bool isComplete)
        {
            var todo = await _todoRepository.Find(new TodoById(id));
            if (todo == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (todo.OwnerId != account.Id)
            {
                return Unauthorized();
            }

            if (isComplete)
            {
                todo.Complete();
            }
            else
            {
                todo.UnComplete();
            }

            await _todoRepository.Update(todo);
            return Ok(todo);
        }

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
        [SwaggerResponse(200, "Returns sucessfully updated Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> AddDueDate(
            [SwaggerParameter("Id of todo to be updated.")] Guid id,
            [FromBody, Required]
            [SwaggerParameter("Due date value to add to todo.")]
                DateTime dueDate)
        {
            return await UpdateDueDate(id, dueDate);
        }

        [HttpDelete("{id}/due")]
        [SwaggerOperation(Summary = "Removes due date a specific todo.", OperationId = "RemoveDueDate")]
        [SwaggerResponse(200, "Returns sucessfully updated Todo.")]
        [SwaggerResponse(400, "No todo found with specified id.")]
        [SwaggerResponse(403, "Don't have access to specific todo.")]
        public async Task<ActionResult<Todo>> RemoveDueDate(
            [SwaggerParameter("Id of todo to be updated.")] Guid id)
        {
            return await UpdateDueDate(id, null);
        }

        private async Task<ActionResult<Todo>> UpdateDueDate(Guid id, DateTime? dueDate)
        {
            var todo = await _todoRepository.Find(new TodoById(id));
            if (todo == null)
            {
                return BadRequest();
            }

            var account = await _accountAccessor.GetAccount();
            if (todo.OwnerId != account.Id)
            {
                return Unauthorized();
            }

            var isPut = Request.Method == "PUT";
            if (isPut && !dueDate.HasValue)
            {
                return BadRequest();
            }

            todo.UpdateDueDate(dueDate);
            await _todoRepository.Update(todo);
            return Ok(todo);
        }
    }
}
