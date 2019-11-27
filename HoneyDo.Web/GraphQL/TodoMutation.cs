using System;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Specifications.Groups;
using HoneyDo.Domain.Specifications.Todos;
using HoneyDo.Web.Models;

namespace HoneyDo.Web.GraphQL
{
    public class TodoMutation
    {
        private readonly IRepository<Todo> _todoRepo;
        private readonly IRepository<Group> _groupRepo;

        public TodoMutation(IRepository<Todo> todoRepo,
            IRepository<Group> groupRepo)
        {
            _todoRepo = todoRepo ?? throw new ArgumentNullException(nameof(todoRepo));
            _groupRepo = groupRepo ?? throw new ArgumentNullException(nameof(groupRepo));
        }

        // TODO create todo

        public async Task<Todo> UpdateTodo(Guid id, TodoCreateForm input)
        {
            var todo = await _todoRepo.Find(new TodoById(id));

            Group group = null;
            if (input.GroupId.HasValue)
            {
                // TODO: only return group user has access too.
                group = await _groupRepo.Find(new GroupById(input.GroupId.Value));
                // TODO verify group exists
                todo.ChangeGroup(group);
            }
            else if (!input.GroupId.HasValue && todo.GroupId.HasValue)
                todo.RemoveGroup();

            if (!string.IsNullOrWhiteSpace(input.Name))
                todo.UpdateName(input.Name);

            if (input.DueDate != todo.DueDate)
                todo.UpdateDueDate(input.DueDate);

            await _todoRepo.Update(todo);
            return todo;
        }

        // TODO delete todo
        // TODO assign/unassign
        // TODO add/remove dueDate
        // TODO add/remove group
    }
}
