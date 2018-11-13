using System;
using HoneyDo.Domain.Entities;
using Xunit;

namespace HoneyDo.Test
{
    public class TodoTest
    {
        [Fact]
        public void Constructor()
        {
            Account account = new Account("test");
            Assert.Throws<ArgumentNullException>(() => new Todo("", account));
            Assert.Throws<ArgumentNullException>(() => new Todo("foobar", null));
            Assert.Throws<ArgumentNullException>(() => new Todo("", null));
            var todo = new Todo("foobar", account);
            Assert.NotEqual(Guid.Empty, todo.Id);
            Assert.Equal("foobar", todo.Name);
            Assert.Equal(DateTime.UtcNow.Year, todo.CreateDate.Year);
            Assert.Equal(DateTime.UtcNow.DayOfYear, todo.CreateDate.DayOfYear);
            Assert.Null(todo.CompletedDate);
            Assert.Null(todo.DueDate);
            Assert.Equal(account.Id, todo.OwnerId);
        }

        [Fact]
        public void UpdateName()
        {
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.UpdateName("blah blah blah");
            Assert.Equal("blah blah blah", todo.Name);
        }

        [Fact]
        public void Complete()
        {
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.Complete();
            Assert.NotNull(todo.CompletedDate);
        }

        [Fact]
        public void UnComplete()
        {
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.Complete();
            todo.UnComplete();
            Assert.Null(todo.CompletedDate);
        }

        [Fact]
        public void UpdateDueDate()
        {
            DateTime date = DateTime.Now;
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.UpdateDueDate(date);
            Assert.Equal(date, todo.DueDate);
        }
    }
}
