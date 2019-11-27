using System;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Todos;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace HoneyDo.Web.GraphQL
{
    public class TaskQuery
    {
        private readonly IRepository<Todo> _repository;

        public TaskQuery(IRepository<Todo> repository)
        {
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Todo> GetTodo(Guid id)
        {
            return await _repository.Find(new TodoById(id));
        }
    }
}
