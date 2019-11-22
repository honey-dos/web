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
            Assert.Equal(DateTime.UtcNow.Year, todo.DateCreated.Year);
            Assert.Equal(DateTime.UtcNow.DayOfYear, todo.DateCreated.DayOfYear);
            Assert.Equal(DateTime.UtcNow.Year, todo.DateModified.Year);
            Assert.Equal(DateTime.UtcNow.DayOfYear, todo.DateModified.DayOfYear);
            Assert.Null(todo.CompletedDate);
            Assert.Null(todo.DueDate);
            Assert.Null(todo.AssigneeId);
            Assert.Null(todo.GroupId);
            Assert.Equal(account.Id, todo.CreatorId);
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
            Account account = new Account("test", "test");
            Group group = new Group("groupName", account);
            var todo = new Todo("foobar", account, group: group);
            Assert.Equal(group, todo.Group);
            Assert.Equal(group.Id, todo.GroupId);
        }

        [Fact]
        public void UpdateName()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            var oldModified = todo.DateModified.ToBinary();
            todo.UpdateName("blah blah blah");
            Assert.Equal("blah blah blah", todo.Name);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }

        [Fact]
        public void Complete()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            var oldModified = todo.DateModified.ToBinary();
            todo.Complete();
            Assert.NotNull(todo.CompletedDate);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }

        [Fact]
        public void UnComplete()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            var oldModified = todo.DateModified.ToBinary();
            todo.Complete();
            todo.UnComplete();
            Assert.Null(todo.CompletedDate);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }

        [Fact]
        public void UpdateDueDate()
        {
            DateTime date = DateTime.Now;
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            var oldModified = todo.DateModified.ToBinary();
            todo.UpdateDueDate(date);
            Assert.Equal(date, todo.DueDate);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }

        [Fact]
        public void Assign()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            var oldModified = todo.DateModified.ToBinary();
            todo.Assign(account);
            Assert.Equal(account.Id, todo.AssigneeId);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }

        [Fact]
        public void Unassign()
        {
            Account account = new Account("test", "test");
            var todo = new Todo("foobar", account);
            var oldModified = todo.DateModified.ToBinary();
            todo.Unassign();
            Assert.Null(todo.AssigneeId);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }

        [Fact]
        public void ChangeGroup()
        {
            Account account = new Account("test", "test");
            Group group = new Group("groupName", account);
            var todo = new Todo("foobar", account);
            var oldModified = todo.DateModified.ToBinary();
            todo.ChangeGroup(group);
	    Assert.Equal(group, todo.Group);
            Assert.Equal(group.Id, todo.GroupId);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }

        [Fact]
        public void RemoveGroup()
        {
            Account account = new Account("test", "test");
            Group group = new Group("groupName", account);
            var todo = new Todo("foobar", account, group: group);
            var oldModified = todo.DateModified.ToBinary();
            todo.RemoveGroup();
            Assert.Null(todo.Group);
            Assert.Null(todo.GroupId);
            Assert.NotEqual(oldModified, todo.DateModified.ToBinary());
        }
    }
}
