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

namespace HoneyDo.Web.Controllers
{
    [Route("api/todos"), Authorize]
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
        public async Task<ActionResult<List<Todo>>> GetTodos()
        {
            var account = await _accountAccessor.GetAccount();
            var todos = await _todoRepository.Query(new TodosForUser(account));
            return Ok(todos);
        }

        [HttpGet("{id}")]
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

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] TodoCreateFormModel model)
        {
            if (!model.IsValid)
            {
                return BadRequest();
            }

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

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodo(Guid id, [FromBody] TodoCreateFormModel model)
        {
            if (!model.IsValid)
            {
                return BadRequest();
            }

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

        [HttpPut("{id}/complete"), HttpDelete("{id}/complete")]
        public async Task<ActionResult<Todo>> Complete(Guid id)
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
            if (isPut)
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

        [HttpPut("{id}/due"), HttpDelete("{id}/due")]
        public async Task<ActionResult<Todo>> Due(Guid id, [FromBody] DateTime? dueDate)
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
