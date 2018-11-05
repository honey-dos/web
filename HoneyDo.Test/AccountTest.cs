using System;
using HoneyDo.Domain.Entities;
using Xunit;

namespace HoneyDo.Test
{
    public class AccountTest
    {
        [Fact]
        public void Constructor()
        {
            Assert.Throws<ArgumentNullException>(() => new Account(""));
            var account = new Account("foobar");
            Assert.NotEqual(Guid.Empty, account.Id);
            Assert.Equal("foobar", account.Name);
            Assert.True(account.IsEnabled);
            Assert.Equal(string.Empty, account.Picture);
        }

        [Fact]
        public void UpdateName()
        {
            var account = new Account("foobar");
			account.UpdateName("blah blah blah");
			Assert.Equal("blah blah blah", account.Name);
        }
    }
}
