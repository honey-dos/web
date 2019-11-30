using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Enums;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Models;
using HoneyDo.Domain.Specifications.Accounts;
using HoneyDo.Domain.Specifications.Groups;
using HoneyDo.Domain.Specifications.Todos;
using HoneyDo.Domain.Values;

namespace HoneyDo.Domain.Services
{
    public class TodoService
    {
        private readonly IRepository<Todo> _todoRepository;
        private readonly IRepository<Account> _accountRepository;
        private readonly IAccountAccessor _accountAccessor;
        private readonly IRepository<Group> _groupRepository;

        public TodoService(IRepository<Todo> todoRepository,
            IAccountAccessor accountAccessor,
            IRepository<Account> accountRepository,
            IRepository<Group> groupRepository)
        {
            _todoRepository = todoRepository;
            _accountAccessor = accountAccessor;
            _accountRepository = accountRepository;
            _groupRepository = groupRepository;
        }

        // TODO add authorization to all methods

        public async Task<List<Todo>> Get() =>
             await _todoRepository.Query(new TodosForAccount(await _accountAccessor.GetAccount()));

        public async Task<Todo> Get(Guid id) =>
            await _todoRepository.Find(new TodoById(id));

        public async Task<List<Todo>> Get(Group group) =>
            await _todoRepository.Query(new TodosForGroup(group));

        public async Task<List<Todo>> Get(Account account) =>
            (await _accountAccessor.GetAccount()).Id == account.Id
                ? await Get()
                : null;

        public async Task<DomainError<Todo>> Create(ITodoForm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Group group = null;
            if (model.GroupId.HasValue)
            {
                // TODO: only return group user has access too.
                group = await _groupRepository.Find(new GroupById(model.GroupId.Value));
                if (group == null)
                    return new DomainError<Todo>(DomainErrorCode.InvalidArgument, nameof(model.GroupId));
            }

            Account assignee = null;
            if (model.AssigneeId.HasValue)
            {
                // TODO restrict assignments to users in group
                assignee = await _accountRepository.Find(new AccountById(model.AssigneeId.Value));
                if (assignee == null)
                    return new DomainError<Todo>(DomainErrorCode.InvalidArgument, nameof(model.AssigneeId));
            }

            var account = await _accountAccessor.GetAccount();
            // TODO verify group exists
            var todo = new Todo(model.Name, account, model.DueDate, group, assignee);
            await _todoRepository.Add(todo);
            return new DomainError<Todo>(todo);
        }

        public async Task<DomainError> Delete(Guid id)
        {
            var todo = await Get(id);
            if (todo == null)
                return DomainError.NotFound();

            var account = await _accountAccessor.GetAccount();
            if (todo.CreatorId != account.Id)
                return DomainError.NotAuthorized();

            await _todoRepository.Remove(todo);
            return DomainError.NoError();
        }

        public async Task<Todo> Update(Guid id, ITodoUpdate model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var todo = await Get(id);

            if (todo == null)
                return null;

            var account = await _accountAccessor.GetAccount();
            if (todo.CreatorId != account.Id)
                return null;

            if (model.GroupId.HasValue &&
                (!todo.GroupId.HasValue || model.GroupId.Value != todo.GroupId.Value))
            {
                // TODO: only return group user has access too.
                var group = await _groupRepository.Find(new GroupById(model.GroupId.Value));
                if (group == null)
                    return null;

                todo.ChangeGroup(group);
            }
            else if (model.RemoveGroup)
            {
                todo.RemoveGroup();
            }

            if (model.AssigneeId.HasValue &&
                (!todo.AssigneeId.HasValue || model.AssigneeId.Value != todo.AssigneeId.Value))
            {
                // TODO restrict assignments to users in group
                account = await _accountRepository.Find(new AccountById(model.AssigneeId.Value));
                if (account == null)
                    return null;

                todo.Assign(account);
            }
            else if (model.RemoveAssignee)
            {
                todo.Unassign();
            }

            if (!string.IsNullOrWhiteSpace(model.Name) && model.Name != todo.Name)
            {
                todo.UpdateName(model.Name);
            }

            if (model.DueDate != todo.DueDate)
            {
                todo.UpdateDueDate(model.DueDate);
            }
            else if (model.RemoveDueDate)
            {
                todo.UpdateDueDate(null);
            }

            await _todoRepository.Update(todo);
            return todo;
        }
    }
}
