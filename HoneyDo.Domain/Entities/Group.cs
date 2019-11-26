using System;
using System.Collections.Generic;

namespace HoneyDo.Domain.Entities
{
    /// <summary>
    /// Honey-Dos group item.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// App unique Id of the Group
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// Name of the Group
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The Id of the creator of the Group
        /// </summary>
        public Guid CreatorId { get; private set; }
        /// <summary>
        /// The date that the Group was created
        /// </summary>
        public DateTime DateCreated { get; private set; }
        /// <summary>
        /// Date the group was last modified.
        /// </summary>
        public DateTime DateModified { get; private set; }
        /// <summary>
        /// Tasks that belong to this group.
        /// </summary>
        protected List<Todo> _tasks { get; set; }
        /// <summary>
        /// Tasks that belong to this group.
        /// </summary>
        public Todo[] Tasks
        {
            get
            {
                if (_tasks == null)
                {
                    return new Todo[0];
                }
                return _tasks.ToArray();
            }
        }
        /// <summary>
        /// User accounts that belong to this group.
        /// </summary>
        protected List<GroupAccount> _groupAccounts { get; set; }
        /// <summary>
        /// User accounts that belong to this group.
        /// </summary>
        public GroupAccount[] GroupAccounts
        {
            get
            {
                if (_groupAccounts == null)
                {
                    return new GroupAccount[0];
                }
                return _groupAccounts.ToArray();
            }
        }

        /// <summary>
        /// Parameterless constructor required for entity framework.
        /// </summary>
        [Obsolete("system constructor")]
        protected Group() { }

        /// <summary>
        /// Create new Group Item
        /// </summary>
        /// <param name="name">Name of the Group</param>
        /// <param name="creator">The Id of the creator of the Group</param>
        public Group(string name, Account creator)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }
            Id = Guid.NewGuid();
            Name = name;
            CreatorId = creator.Id;
            DateCreated = DateModified = DateTime.UtcNow;
            _tasks = new List<Todo>();
            _groupAccounts = new List<GroupAccount>();
        }

        /// <summary>
        /// Update the groups's name.
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
    }
}
