using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HoneyDo.Domain.Entities;
using HoneyDo.Web.Models;

namespace HoneyDo.Web.Controllers
{
    [Route("api/todos")]
    public class TodoController : Controller
    {
        private static Todo[] Todos= new[]
        {
			new Todo("Fix Jest Unit Tests"),
			new Todo("Add Persistence"),
			new Todo("Rake in the money.")
        };

        [HttpGet]
        public IActionResult GetTodos()
        {
			return Ok(Todos);
        }

        [HttpGet("{id}")]
        public IActionResult GetTodo(Guid id)
        {
			var todo = Todos.FirstOrDefault(t => t.Id == id);
            return Ok(todo);
        }

        [HttpPost]
		public IActionResult CreateTodo([FromBody] TodoCreateFormModel model)
		{
            if (!model.IsValid)
            {
                return BadRequest();
            }

            var todo = new Todo(model.Name);
            return Created($"api/todos/{todo.Id}", todo);
		}

        [HttpDelete("{id}")]
        public IActionResult DeleteTodo(Guid id)
        {
			return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTodo(Guid id, [FromBody] TodoCreateFormModel model)
        {
            if (!model.IsValid)
            {
                return BadRequest();
            }

			var todo = Todos.FirstOrDefault(t => t.Id == id);
			todo.UpdateName(model.Name);
            return Ok(todo);
        }
    }
}
