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
            Account account = new Account("test");
            Assert.Throws<ArgumentNullException>(() => new Group("", account));
            Assert.Throws<ArgumentNullException>(() => new Group("foobar", null));
            Assert.Throws<ArgumentNullException>(() => new Group("", null));
            var group = new Group("Gang", account);
            Assert.NotEqual(Guid.Empty,group.Id);
            Assert.Equal("Gang", group.Name);
            Assert.Equal(account.Id, group.CreatorId);
            Assert.Equal(DateTime.UtcNow.Year, group.CreateDate.Year);
            Assert.Equal(DateTime.UtcNow.DayOfYear, group.CreateDate.DayOfYear);
        }
    }
}