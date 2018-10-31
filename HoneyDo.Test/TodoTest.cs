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
            Assert.Throws<ArgumentNullException>(() => new Todo(""));
            var todo = new Todo("foobar");
            Assert.NotEqual(Guid.Empty, todo.Id);
            Assert.Equal("foobar", todo.Name);
        }

        [Fact]
        public void UpdateName()
        {
            var todo = new Todo("foobar");
			todo.UpdateName("blah blah blah");
			Assert.Equal("blah blah blah", todo.Name);
        }
    }
}
