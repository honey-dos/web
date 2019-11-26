using System;
using System.Collections.Generic;

namespace HoneyDo.Domain.Entities
{
    /// <summary>
    /// Honey-Dos User Account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Id of the account.
        /// </summary>
        public Guid Id { get; private set; }
        /// <summary>
        /// User's given name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// True if the account is enabled, false if disabled.
        /// </summary>
        public bool IsEnabled { get; private set; }
        /// <summary>
        /// Url to user's picture.
        /// </summary>
        public string Picture { get; private set; }
        /// <summary>
        /// Accounts app unique username
        /// </summary>
        public string UserName { get; private set; }
        /// <summary>
        /// Accounts app unique normalized username
        /// </summary>
        public string NormalizedUserName { get; private set; }
        /// <summary>
        /// Todos that belong to this group.
        /// </summary>
        protected List<GroupAccount> _groupAccounts { get; set; }
        /// <summary>
        /// GroupAccounts that belong to this account
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
        /// Create new account.
        /// </summary>
        /// <param name="name">User's given name.</param>
        /// <param name="userName">User's app name, app unique.</param>
        /// <param name="picture">User's picture (optional).</param>
        public Account(string name, string userName, string picture = "")
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            Id = Guid.NewGuid();
            Name = name.Trim();
            IsEnabled = true;
            Picture = picture;
            UserName = userName;
            NormalizedUserName = userName.ToUpper();
            _groupAccounts = new List<GroupAccount>();
        }

        /// <summary>
        /// Disable the account.
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
        }

        /// <summary>
        /// Enable the account.
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Update the user's given name.
        /// </summary>
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name.Trim();
        }

        /// <summary>
        /// Update the user's picture.
        /// </summary>
        public void UpdatePicture(string picture)
        {
            if (string.IsNullOrWhiteSpace(picture))
                throw new ArgumentNullException(nameof(picture));

            Picture = picture.Trim();
        }

        /// <summary>
        /// Update the user's user name
        /// </summary>
        public void UpdateUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            UserName = userName.Trim();
        }

        /// <summary>
        /// Update the user's normalized user name
        /// </summary>
        public void UpdateNormalizedUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));

            NormalizedUserName = userName;
        }
    }
}
