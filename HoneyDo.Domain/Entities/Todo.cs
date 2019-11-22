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
        public Guid CreatorId { get; private set; }
        /// <summary>
        /// Id of the user who is assigned to the todo.
        /// </summary>
        public Guid? AssigneeId { get; private set; }
        /// <summary>
        /// Date the todo was created.
        /// </summary>
        public DateTime DateCreated { get; private set; }
        /// <summary>
        /// Date the todo was last modified.
        /// </summary>
        public DateTime DateModified { get; private set; }
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
        /// The group the todo belongs to.
        /// </summary>
        public Group Group { get; private set; }

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
            CreatorId = owner.Id;
            DateCreated = DateModified = DateTime.UtcNow;
            DueDate = dueDate;
            if (group != null)
            {
                Group = group;
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
            DateModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Complete the todo.
        /// </summary>
        public void Complete()
        {
            CompletedDate = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Uncomplete the todo.
        /// </summary>
        public void UnComplete()
        {
            CompletedDate = null;
            DateModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Update the todo's due date.
        /// </summary>
        /// <param name="dueDate">New due date value.</param>
        public void UpdateDueDate(DateTime? dueDate)
        {
            DueDate = dueDate;
            DateModified = DateTime.UtcNow;
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
            DateModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Unassigns the todo.
        /// </summary>
        public void Unassign()
        {
            AssigneeId = null;
            DateModified = DateTime.UtcNow;
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
            Group = group;
            GroupId = group.Id;
            DateModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Remove group
        /// </summary>
        public void RemoveGroup()
        {
            Group = null;
            GroupId = null;
            DateModified = DateTime.UtcNow;
        }
    }
}
