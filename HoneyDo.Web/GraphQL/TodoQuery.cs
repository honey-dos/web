using System;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Todos;
using HotChocolate.Resolvers;

namespace HoneyDo.Web.GraphQL
{
    public class TodoQuery
    {
        private readonly IRepository<Todo> _todoRepo;
        private readonly IAccountAccessor _accountAccessor;

        public TodoQuery(IRepository<Todo> todoRepo,
            IAccountAccessor accountAccessor)
        {
            _todoRepo = todoRepo ?? throw new ArgumentNullException(nameof(todoRepo));
            _accountAccessor = accountAccessor ?? throw new ArgumentNullException(nameof(accountAccessor));
        }

        // TODO get all todos
        public async Task<Todo[]> GetTodos(IResolverContext context)
        {
            var account = await _accountAccessor.GetAccount("_groupAccounts.Group._tasks");
            return account.Tasks;
        }

        public async Task<Todo> GetTodo(Guid id)
        {
            return await _todoRepo.Find(new TodoById(id));
        }
    }
}
