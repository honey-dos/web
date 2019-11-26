using System;
using HoneyDo.Domain.Entities;
using Xunit;

namespace HoneyDo.Test
{
    public class GroupAccountTest
    {
        [Fact]
        public void Constructor()
        {
            Account account = new Account("test", "test");
            var group = new Group("Gang", account);

            Assert.Throws<ArgumentNullException>(() => new GroupAccount(null, account));
            Assert.Throws<ArgumentNullException>(() => new GroupAccount(group, null));

            var groupAccount = new GroupAccount(group, account);
            Assert.Equal(group.Id, groupAccount.GroupId);
            Assert.Equal(account.Id, groupAccount.AccountId);
        }
    }
}
