using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Services;
using HoneyDo.Web.Extensions;
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
        public async Task<Todo> Todo(Guid todoId, IResolverContext ctx) =>
            (await _todoService.Get(todoId)).ForGraphQL(ctx);

        /// <summary> Create todo. </summary>
        public async Task<Todo> CreateTodo(TodoCreateForm input, IResolverContext ctx) =>
            (await _todoService.Create(input)).ForGraphQL(ctx);

        /// <summary> Update todo. </summary>
        public async Task<Todo> UpdateTodo(Guid todoId, TodoUpdateForm input, IResolverContext ctx) =>
            (await _todoService.Update(todoId, input)).ForGraphQL(ctx);

        /// <summary> Delete todo. </summary>
        public async Task<bool> DeleteTodo(Guid todoId, IResolverContext ctx) =>
            (await _todoService.Delete(todoId)).ForGraphQL(ctx);
    }
}
