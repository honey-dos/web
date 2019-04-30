using System;

namespace HoneyDo.Domain.Entities
{
    /// <summary>
    /// Honey-Dos todo item.
    /// </summary>
    public class Todo
    {
        /// <summary>
        /// App unique id of todo.
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// String by which the todo item is known by.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Id of the user who created the todo.
        /// </summary>
        public Guid OwnerId { get; private set; }
        /// <summary>
        /// Id of the user who is assigned to the todo.
        /// </summary>
        public Guid? AssigneeId { get; private set; }
        /// <summary>
        /// Date the todo was created.
        /// </summary>
        public DateTime CreateDate { get; private set; }
        /// <summary>
        /// Date the todo was completed.
        /// </summary>
        public DateTime? CompletedDate { get; private set; }
        /// <summary>
        /// Date the todo should be completed.
        /// </summary>
        public DateTime? DueDate { get; private set; }
        /// <summary>
        /// Group Id of the group the todo belongs to.
        /// </summary>
        public Guid? GroupId { get; private set; }

        /// <summary>
        /// Parameterless constructor required for entity framework.
        /// </summary>
        [Obsolete("system constructor")]
        protected Todo() { }

        /// <summary>
        /// Create new todo item.
        /// </summary>
        /// <param name="name">Text by which the todo will be known.</param>
        /// <param name="owner">User who creating the todo.</param>
        /// <param name="dueDate">Optional date the todo should be completed by.</param>
        /// <param name="group">Optional group for the group the todo belongs to.</param>
        public Todo(string name, Account owner, DateTime? dueDate = null, Group group = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (owner == null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            Id = Guid.NewGuid();
            Name = name;
            OwnerId = owner.Id;
            CreateDate = DateTime.UtcNow;
            DueDate = dueDate;
            if (group != null)
            {
                GroupId = group.Id;
            }
        }

        /// <summary>
        /// Update the todo's name.
        /// </summary>
        /// <param name="name">New name</param>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        /// <summary>
        /// Complete the todo.
        /// </summary>
        public void Complete()
        {
            CompletedDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Uncomplete the todo.
        /// </summary>
        public void UnComplete()
        {
            CompletedDate = null;
        }

        /// <summary>
        /// Update the todo's due date.
        /// </summary>
        /// <param name="dueDate">New due date value.</param>
        public void UpdateDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
        }

        /// <summary>
        /// Assigns the todo to the account given.
        /// </summary>
        /// <param name="assignee">New account the todo will be assigned to.</param>
        public void Assign(Account assignee)
        {
            if (assignee == null)
            {
                throw new ArgumentNullException(nameof(assignee));
            }
            AssigneeId = assignee.Id;
        }

        /// <summary>
        /// Unassigns the todo.
        /// </summary>
        public void Unassign()
        {
            AssigneeId = null;
        }

        /// <summary>
        /// Moves the todo to the specified group ID.
        /// </summary>
        /// <param name="group">New account the todo will belong to.</param>
        public void ChangeGroup(Group group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }
            GroupId = group.Id;
        }

        /// <summary>
        /// Remove group
        /// </summary>
        public void RemoveGroup()
        {
            GroupId = null;
        }
    }
}
