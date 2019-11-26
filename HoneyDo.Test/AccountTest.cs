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
            var account = new Account("tyler evans", "foobar");

            Assert.Throws<ArgumentNullException>(() => new Account("", "foobar"));
            Assert.Throws<ArgumentNullException>(() => new Account("foobar", ""));

            Assert.NotEqual(Guid.Empty, account.Id);
            Assert.Equal("tyler evans", account.Name);
            Assert.True(account.IsEnabled);
            Assert.Equal(string.Empty, account.Picture);
            Assert.Empty(account.Tasks);
        }

        [Fact]
        public void SecondaryConstructor()
        {
            var account = new Account("tyler evans", "foobar", "some picture");

            Assert.NotEqual(Guid.Empty, account.Id);
            Assert.Equal("tyler evans", account.Name);
            Assert.True(account.IsEnabled);
            Assert.Equal("some picture", account.Picture);
        }

        [Fact]
        public void Disable()
        {
            var account = new Account("tyler evans", "foobar");

            account.Disable();

            Assert.False(account.IsEnabled);
        }

        [Fact]
        public void Enable()
        {
            var account = new Account("tyler evans", "foobar");

            account.Disable();
            account.Enable();

            Assert.True(account.IsEnabled);
        }

        [Fact]
        public void UpdateAvatar()
        {
            var account = new Account("tyler evans", "foobar");

            account.UpdatePicture("foobar");

            Assert.Equal("foobar", account.Picture);
        }

        [Fact]
        public void UpdateName()
        {
            var account = new Account("tyler evans", "foobar");

            account.UpdateName("blah");

            Assert.Equal("blah", account.Name);
        }
    }
}
