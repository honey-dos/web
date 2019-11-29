using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Services;
using HoneyDo.Web.Models;
using HotChocolate.Resolvers;

namespace HoneyDo.Web.GraphQL.Resolvers
{
    public class TodoResolver
    {
        private readonly TodoService _todoService;

        public TodoResolver(TodoService todoService)
        {
            _todoService = todoService ?? throw new ArgumentNullException(nameof(todoService));
        }

        /// <summary> Gets all the todos that the user has access to.</summary>
        public async Task<List<Todo>> Todos() => await _todoService.Get();

        /// <summary> Gets a specific todo.  </summary>
        public async Task<Todo> Todo(Guid todoId) => await _todoService.Get(todoId);

        /// <summary> Create todo. </summary>
        public async Task<Todo> CreateTodo(TodoCreateForm input) => await _todoService.Create(input);

        /// <summary> Update todo. </summary>
        public async Task<Todo> UpdateTodo(Guid todoId, TodoUpdateForm input, IResolverContext context) =>
            await _todoService.Update(todoId, input);

        /// <summary> Delete todo. </summary>
        public async Task<bool> Delete(Guid todoId) => await _todoService.DeleteTodo(todoId);
    }
}
