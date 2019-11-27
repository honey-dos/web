using System;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Todos;

namespace HoneyDo.Web.GraphQL
{
    public class TodoQuery
    {
        private readonly IRepository<Todo> _repository;

        public TodoQuery(IRepository<Todo> repository)
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
