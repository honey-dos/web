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
            Account account = new Account("test", "test");
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
            Assert.Null(todo.AssigneeId);
            Assert.Null(todo.GroupId);
            Assert.Equal(account.Id, todo.OwnerId);
        }

        [Fact]
        public void ConstructorWithDate()
        {
            Account account = new Account("test", "test");
            DateTime dueDate = new DateTime(2019, 6, 28);
            var todo = new Todo("foobar", account, dueDate: dueDate);
            Assert.Equal(dueDate, todo.DueDate);
        }

        [Fact]
        public void ConstructorWithGroupId()
        {
            Account account = new Account("test");
            Guid? groupId = new Guid();
            var todo = new Todo("foobar", account, groupId: groupId);
            Assert.Equal(groupId, todo.GroupId);
        }

        [Fact]
        public void UpdateName()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            todo.UpdateName("blah blah blah");
            Assert.Equal("blah blah blah", todo.Name);
        }

        [Fact]
        public void Complete()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            todo.Complete();
            Assert.NotNull(todo.CompletedDate);
        }

        [Fact]
        public void UnComplete()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            todo.Complete();
            todo.UnComplete();
            Assert.Null(todo.CompletedDate);
        }

        [Fact]
        public void UpdateDueDate()
        {
            DateTime date = DateTime.Now;
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            todo.UpdateDueDate(date);
            Assert.Equal(date, todo.DueDate);
        }

        [Fact]
        public void Assign()
        {
            Guid assigneeId = new Guid();
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.Assign(assigneeId);
            Assert.Equal(assigneeId, todo.AssigneeId);
        }

        [Fact]
        public void AssignNull()
        {
            Guid? assigneeId = null;
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.Assign(assigneeId);
            Assert.Null(todo.AssigneeId);
        }

        [Fact]
        public void ChangeGroup()
        {
            Guid groupId = new Guid();
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.ChangeGroup(groupId);
            Assert.Equal(groupId, todo.GroupId);
        }

        [Fact]
        public void ChangeGroupNull()
        {
            Guid? groupId = null;
            Account account = new Account("test");
            var todo = new Todo("foobar", account);
            todo.ChangeGroup(groupId);
            Assert.Null(todo.GroupId);
        }
    }
}
