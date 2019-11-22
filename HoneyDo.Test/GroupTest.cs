using System;
using HoneyDo.Domain.Entities;
using Xunit;

namespace HoneyDo.Test
{
    public class GroupTest
    {
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
    }
}
