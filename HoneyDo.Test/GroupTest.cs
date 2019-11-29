using System;
using System.Linq;
using HoneyDo.Domain.Entities;
using Xunit;

namespace HoneyDo.Test
{
    public class GroupTest
    {
        private readonly Account[] _accounts = new Account[]
        {
          new Account("Luke Skywalker", "ls"),
          new Account("Han Solo", "hs"),
          new Account("Leia Organa", "lo")
        };

        [Fact]
        public void Constructor()
        {
            Account account = new Account("test", "test");
            Assert.Throws<ArgumentNullException>(() => new Group("", account));
            Assert.Throws<ArgumentNullException>(() => new Group("foobar", null));
            Assert.Throws<ArgumentNullException>(() => new Group("", null));
            var group = new Group("Gang", account);
            Assert.NotEqual(Guid.Empty, group.Id);
            Assert.Equal("Gang", group.Name);
            Assert.Equal(account.Id, group.CreatorId);
            Assert.Equal(DateTime.UtcNow.Year, group.DateCreated.Year);
            Assert.Equal(DateTime.UtcNow.DayOfYear, group.DateCreated.DayOfYear);
            Assert.Equal(DateTime.UtcNow.Year, group.DateModified.Year);
            Assert.Equal(DateTime.UtcNow.DayOfYear, group.DateModified.DayOfYear);
            Assert.Empty(group.Tasks);
            Assert.Collection(group.Accounts, x => Assert.Equal(account, x));
        }

        [Fact]
        public void UpdateName()
        {
            Account account = new Account("test", "test");
            var group = new Group("Gang", account);
            var oldModified = group.DateModified.ToBinary();
            group.UpdateName("blah blah blah");
            Assert.Equal("blah blah blah", group.Name);
            Assert.NotEqual(oldModified, group.DateModified.ToBinary());
        }

        [Fact]
        public void AddAccounts()
        {
            Account account = new Account("test", "test");
            var group = new Group("Gang", account);
            Assert.Throws<ArgumentException>(() => group.AddAccounts(new Account[] { account }));
            var groupAccounts = group.AddAccounts(_accounts);
            Assert.Equal(4, group.Accounts.Length);
            Assert.Equal(3, groupAccounts.Length);
            var ids = _accounts.Select(a => a.Id).ToList();
            ids.Add(account.Id);
            Assert.All(group.Accounts, x => Assert.Contains(x.Id, ids));
        }

        [Fact]
        public void RemoveAccounts()
        {
            Account account = new Account("test", "test");
            var group = new Group("Gang", account);
            Assert.Throws<ArgumentException>(() => group.RemoveAccounts(new Guid[] { _accounts[0].Id }));
            group.AddAccounts(_accounts);
            var removeAccounts = new Account[]
            {
              _accounts[1],
              _accounts[2]
            };
            var ids = removeAccounts.Select(a => a.Id).ToArray();
            group.RemoveAccounts(ids);
            Assert.Equal(2, group.Accounts.Length);
            Assert.All(group.Accounts, x => Assert.DoesNotContain(x.Id, ids));
        }
    }
}
