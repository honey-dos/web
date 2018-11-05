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
        public IActionResult GetTodos()
        {
            return Ok(_todoRepository.Query(new TodosForUser()));
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo(Guid id)
        {
            var todo = _todoRepository.Find(new TodoById(id));
            return Ok(todo);
        }

        [HttpPost]
        public IActionResult CreateTodo([FromBody] TodoCreateFormModel model)
        {
            if (!model.IsValid)
            {
                return BadRequest();
            }

            // var account = _accountAccessor.Account;
            var todo = new Todo(model.Name);
            _todoRepository.Add(todo);
            return Created($"api/todos/{todo.Id}", todo);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(Guid id)
        {
            var todo = _todoRepository.Find(new TodoById(id));
            if (todo == null)
            {
                return BadRequest();
            }
            _todoRepository.Remove(todo);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(Guid id, [FromBody] TodoCreateFormModel model)
        {
            if (!model.IsValid)
            {
                return BadRequest();
            }

            var todo = _todoRepository.Find(new TodoById(id));

            if (todo == null)
            {
                return BadRequest();
            }

            todo.UpdateName(model.Name);
            _todoRepository.Update(todo);
            return Ok(todo);
        }
    }
}
