
using System;
using System.Linq.Expressions;
using HoneyDo.Domain.Entities;
using HoneyDo.Domain.Specifications.Todos;
using Xunit;

namespace HoneyDo.Test
{
    public class TodoByIdTest
    {
        [Fact]
        public void BuildExpression()
        {
            var account = new Account("foobar");
            var newTodo = new Todo("test todo",account);
            Expression<Func<Todo, bool>> expr = todo => todo.Id == account.Id;
            var todoById = new TodoById(account.Id);
            var result = todoById.BuildExpression().Compile()(newTodo);
            Assert.True(result);
        }
    }
}