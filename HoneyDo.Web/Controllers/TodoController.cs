using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneyDo.Domain.Entities;
using HoneyDo.Web.Models;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Todos;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace HoneyDo.Web.Controllers
{
    /// <summary>
    /// Controller to handle Todos (Tasks)
    /// </summary>
    [Route("api/todos"), Authorize, ApiController]
    [ProducesResponseType(401)]
    [Produces("application/json")]
    public class TodoController : Controller
    {
        private readonly IRepository<Todo> _todoRepository;
        private readonly IAccountAccessor _accountAccessor;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="todoRepository">Todo repository, todo persistence.</param>
        /// <param name="accountAccessor">Account accessor, used to determine logged in user's account</param>
        public TodoController(IRepository<Todo> todoRepository, IAccountAccessor accountAccessor)
        {
            _todoRepository = todoRepository;
            _accountAccessor = accountAccessor;
        }

        /// <summary>
        /// Gets all the todos the user as access to.
        /// </summary>
        /// <returns>List of Todos</returns>
        /// <response code="200">Returns all the Todos for the user.</response>
        /// <response code="401">Not logged in.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<List<Todo>>> GetTodos()
        {
            var account = await _accountAccessor.GetAccount();
            var todos = await _todoRepository.Query(new TodosForUser(account));
            return Ok(todos);
        }

        /// <summary>
        /// Gets a specific todo.
        /// </summary>
        /// <param name="id">Id of the todo.</param>
        /// <returns>Todo</returns>
        /// <response code="200">Returns the Todo.</response>
        /// <response code="400">Todo not found with id not found.</response>
        /// <response code="401">Not logged in, or user doesn't have access.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Todo>> GetTodo(Guid id)
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

        /// <summary>
        /// Create a new todo
        /// </summary>
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
        /// <param name="model">Todo values, optional: dueDate.</param>
        /// <returns>A newly created Todo.</returns>
        /// <response code="200">Returns the newly created Todo.</response>
        /// <response code="401">Not logged in.</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] TodoCreateFormModel model)
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

        /// <summary>
        /// Deletes a specific todo.
        /// </summary>
        /// <param name="id">Id of todo to be deleted.</param>
        /// <returns></returns>
        /// <response code="204">Todo was successfully deleted.</response>
        /// <response code="400">No todo found.</response>
        /// <response code="401">Don't have access to delete this todo.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeleteTodo(Guid id)
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

        /// <summary>
        /// Update a specific todo.
        /// </summary>
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
        /// <param name="id">Id of todo to update.</param>
        /// <param name="model">Values to update the todo to.</param>
        /// <returns>The updated todo.</returns>
        /// <response code="200">Returns the updated Todo.</response>
        /// <response code="400">No todo found.</response>
        /// <response code="401">Don't have access to delete this todo.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Todo>> UpdateTodo(Guid id, [FromBody] TodoCreateFormModel model)
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

        /// <summary>
        /// Complete a todo.
        /// </summary>
        /// <param name="id">Id of todo to (un)complete.</param>
        /// <returns></returns>
        /// <response code="200">Returns the updated Todo.</response>
        /// <response code="400">No todo found.</response>
        /// <response code="401">Don't have access to delete this todo.</response>
        [HttpPut("{id}/complete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Todo>> Complete(Guid id)
        {
            return await SetCompletion(id, true);
        }

        /// <summary>
        /// Uncomplete a todo.
        /// </summary>
        /// <param name="id">Id of todo to (un)complete.</param>
        /// <returns></returns>
        /// <response code="200">Returns the updated Todo.</response>
        /// <response code="400">No todo found.</response>
        /// <response code="401">Don't have access to delete this todo.</response>
        [HttpDelete("{id}/complete")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Todo>> Uncomplete(Guid id)
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

        /// <summary>
        /// Add due date to todo.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/todos/{id}
        ///     "2018-11-13T23:53:09.651Z"
        ///
        /// </remarks> 
        /// <param name="id">Id of todo to add due date.</param>
        /// <param name="dueDate">Due date value to add to todo.</param>
        /// <returns></returns>
        /// <response code="200">Returns the updated Todo.</response>
        /// <response code="400">No todo found, or dueDate not given.</response>
        /// <response code="401">Don't have access to delete this todo.</response>
        [HttpPut("{id}/due")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Todo>> AddDueDate(Guid id, [FromBody, Required] DateTime dueDate)
        {
            return await UpdateDueDate(id, dueDate);
        }

        /// <summary>
        /// Remove due date from todo.
        /// </summary>
        /// <param name="id">Id of todo to remove due date.</param>
        /// <returns></returns>
        /// <response code="200">Returns the updated Todo.</response>
        /// <response code="400">No todo found.</response>
        /// <response code="401">Don't have access to delete this todo.</response>
        [HttpDelete("{id}/due")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Todo>> RemoveDueDate(Guid id)
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
