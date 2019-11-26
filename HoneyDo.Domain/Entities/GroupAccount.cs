using System;

namespace HoneyDo.Domain.Entities
{
    /// <summary>
    /// Honey-Dos Group Account.
    /// </summary>
    public class GroupAccount
    {
        /// <summary>
        /// The id of the group that the account belongs too.
        /// </summary>
        public Guid GroupId { get; private set; }
        /// <summary>
        /// The id of the account the group contains
        /// </summary>
        public Guid AccountId { get; private set; }
        /// <summary>
        /// Honey-Dos Group item.
        /// </summary>
        public Group Group { get; private set; }
        /// <summary>
        /// Honey-Dos User Account.
        /// </summary>
        public Account Account { get; private set; }

        /// <summary>
        /// Parameterless constructor required for entity framework.
        /// </summary>
        [Obsolete("System constructor")]
        protected GroupAccount() { }

        /// <summary>
        /// Create new group account.
        /// </summary>
        /// <param name="group">Group the account will be added too.</param>
        /// <param name="account">User the group will receive.</param>
        public GroupAccount(Group group, Account account)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            GroupId = group.Id;
            AccountId = account.Id;
            Group = group;
            Account = account;
        }
    }
}
