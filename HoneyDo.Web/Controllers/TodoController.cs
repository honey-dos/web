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
        public async Task<IActionResult> GetTodos()
        {
            var todos = await _todoRepository.Find(new TodosForUser());
            return Ok(todos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(Guid id)
        {
            var todo = await _todoRepository.Find(new TodoById(id));
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoCreateFormModel model)
        {
            if (!model.IsValid)
            {
                return BadRequest();
            }

            // var account = _accountAccessor.Account;
            var todo = new Todo(model.Name);
            await _todoRepository.Add(todo);
            return Created($"api/todos/{todo.Id}", todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(Guid id)
        {
            var todo = await _todoRepository.Find(new TodoById(id));
            if (todo == null)
            {
                return BadRequest();
            }
            await _todoRepository.Remove(todo);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(Guid id, [FromBody] TodoCreateFormModel model)
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

            todo.UpdateName(model.Name);
            await _todoRepository.Update(todo);
            return Ok(todo);
        }
    }
}
