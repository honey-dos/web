using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Interfaces;
using HoneyDo.Domain.Models;
using HoneyDo.Domain.Specifications.Accounts;
using HoneyDo.Domain.Specifications.Groups;
using HoneyDo.Domain.Specifications.Todos;
using HoneyDo.Domain.Values.Errors;

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

        public async Task<IDomainResult<Todo>> Get(Guid id)
        {
            var todo = await _todoRepository.Find(new TodoById(id));
            if (todo == null)
                return new NotFoundResult<Todo>();

            return new SuccessResult<Todo>(todo);
        }

        public async Task<List<Todo>> Get(Group group) =>
            await _todoRepository.Query(new TodosForGroup(group));

        public async Task<List<Todo>> Get(Account account) =>
            (await _accountAccessor.GetAccount()).Id == account.Id
                ? await Get()
                : null;

        public async Task<IDomainResult<Todo>> Create(ITodoForm input)
        {
            if (input == null)
                return new InvalidArgumentResult<Todo>(nameof(input));

            Group group = null;
            if (input.GroupId.HasValue)
            {
                // TODO: only return group user has access too.
                group = await _groupRepository.Find(new GroupById(input.GroupId.Value));
                if (group == null)
                    return new InvalidArgumentResult<Todo>(nameof(input.GroupId));
            }

            Account assignee = null;
            if (input.AssigneeId.HasValue)
            {
                // TODO restrict assignments to users in group
                assignee = await _accountRepository.Find(new AccountById(input.AssigneeId.Value));
                if (assignee == null)
                    return new InvalidArgumentResult<Todo>(nameof(input.AssigneeId));
            }

            var account = await _accountAccessor.GetAccount();
            // TODO verify group exists
            var todo = new Todo(input.Name, account, input.DueDate, group, assignee);
            await _todoRepository.Add(todo);
            return new CreatedResult<Todo>(todo);
        }

        public async Task<IDomainResult> Delete(Guid id)
        {
            var result = await Get(id);
            if (result.HasError)
                return result as IDomainResult;

            var todo = result.Value;
            var account = await _accountAccessor.GetAccount();
            if (todo.CreatorId != account.Id)
                return new InsufficientPermissionsResult();

            await _todoRepository.Remove(todo);
            return new DeletedResult();
        }

        public async Task<IDomainResult<Todo>> Update(Guid id, ITodoUpdate input)
        {
            if (input == null)
                return new InvalidArgumentResult<Todo>(nameof(input));

            var result = await Get(id);
            if (result.HasError)
                return result;

            var todo = result.Value;
            var account = await _accountAccessor.GetAccount();
            if (todo.CreatorId != account.Id)
                return new InsufficientPermissionsResult<Todo>();

            if (input.GroupId.HasValue &&
                (!todo.GroupId.HasValue || input.GroupId.Value != todo.GroupId.Value))
            {
                // TODO: only return group user has access too.
                var group = await _groupRepository.Find(new GroupById(input.GroupId.Value));
                if (group == null)
                    return new InvalidArgumentResult<Todo>(nameof(input.GroupId));

                todo.ChangeGroup(group);
            }
            else if (input.RemoveGroup)
            {
                todo.RemoveGroup();
            }

            if (input.AssigneeId.HasValue &&
                (!todo.AssigneeId.HasValue || input.AssigneeId.Value != todo.AssigneeId.Value))
            {
                // TODO restrict assignments to users in group
                account = await _accountRepository.Find(new AccountById(input.AssigneeId.Value));
                if (account == null)
                    return new InvalidArgumentResult<Todo>(nameof(input.AssigneeId));

                todo.Assign(account);
            }
            else if (input.RemoveAssignee)
            {
                todo.Unassign();
            }

            if (!string.IsNullOrWhiteSpace(input.Name) && input.Name != todo.Name)
            {
                todo.UpdateName(input.Name);
            }

            if (input.DueDate != todo.DueDate)
            {
                todo.UpdateDueDate(input.DueDate);
            }
            else if (input.RemoveDueDate)
            {
                todo.UpdateDueDate(null);
            }

            await _todoRepository.Update(todo);
            return new SuccessResult<Todo>(todo);
        }
    }
}
